using AutoMapper;
using Carnets.Application.Gympasses.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using Common.Models.Dtos;
using MediatR;

namespace Carnets.Application.Gympasses.Commands
{
    public record CreateGympassCommand : IRequest<Result<GympassWithSessionDto>>
    {
        public string UserId { get; init; }
        public CreateGympassDto Model { get; init; }
    }

    public class CreateGympassCommandHandler : IRequestHandler<CreateGympassCommand, Result<GympassWithSessionDto>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly IPaymentService _paymentService;
        private readonly IMembershipService _membershipService;
        private readonly IMapper _mapper;

        public CreateGympassCommandHandler(
            IGympassRepository gympassRepository,
            IPaymentService paymentService,
            IMembershipService membershipService, 
            IMapper mapper)
        {
            _gympassRepository = gympassRepository;
            _paymentService = paymentService;
            _membershipService = membershipService;
            _mapper = mapper;
        }

        public async Task<Result<GympassWithSessionDto>> Handle(CreateGympassCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                throw new ArgumentException(nameof(request.UserId));
            }

            var created = new Gympass
            {
                GympassId = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Status = GympassStatus.New,
                ActivationDate = DateTime.MinValue,
                ValidityDate = DateTime.MinValue,
            };

            var createResult = await _gympassRepository.CreateGympass(request.Model.GympassTypeId, created);

            if (createResult.IsSuccess)
            {
                var gympassTypeId = createResult.Value.GympassType.GympassTypeId;
                var gympassId = createResult.Value.GympassId;
                var customerId = await _paymentService.GetOrCreateCustomer(request.UserId);

                var checkoutSessionUrl = await _paymentService.CreateCheckoutSession(gympassId, customerId,
                    gympassTypeId, request.Model.SuccessUrl, request.Model.CancelUrl);

                await _gympassRepository.SaveChangesAsync();
                await _membershipService.CreateMembership(new CreateMembershipDto
                {
                    MemberId = request.UserId,
                    FitnessClubId = createResult.Value.GympassType.FitnessClubId
                });

                var gympassWithSession = _mapper.Map<GympassWithSessionDto>(createResult.Value);
                gympassWithSession.CheckoutSessionUrl = checkoutSessionUrl;

                return new Result<GympassWithSessionDto>(gympassWithSession);
            }

            return new Result<GympassWithSessionDto>(createResult.Errors);
        }
    }
}
