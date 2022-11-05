using AutoMapper;
using Carnets.Application.Models;
using Carnets.Application.Gympasses.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using Common.Models.Dtos;
using MediatR;
using Carnets.Application.Enums;

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
                ActivationDate = DateTime.UtcNow,
                ValidityDate = DateTime.UtcNow,
                PaymentType = request.Model.PaymentType
            };

            var createResult = await _gympassRepository.CreateGympass(request.Model.GympassTypeId, created);

            if (createResult.IsSuccess)
            {
                var checkoutSessionUrl = await CreateCheckoutSession(createResult.Value, request);

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

        public async Task<string> CreateCheckoutSession(Gympass createdGympass, CreateGympassCommand request)
        {
            var priceId = createdGympass.PaymentType switch
            {
                PaymentType.OneTime => createdGympass.GympassType.OneTimePriceId,
                PaymentType.Recurring => createdGympass.GympassType.ReccuringPriceId,
                _ => throw new ArgumentOutOfRangeException(nameof(createdGympass.PaymentType))
            };

            var paymentModeType = createdGympass.PaymentType switch
            {
                PaymentType.OneTime => PaymentModeType.payment,
                PaymentType.Recurring => PaymentModeType.subscription,
                _ => throw new ArgumentOutOfRangeException(nameof(createdGympass.PaymentType))
            };

            var checkoutSessionParams = new CheckoutSessionParams(
                gympassId: createdGympass.GympassId,
                customerId: await _paymentService.GetOrCreateCustomer(request.UserId),
                gympassTypeId: createdGympass.GympassType.GympassTypeId,
                successUrl: request.Model.SuccessUrl,
                cancelUrl: request.Model.CancelUrl,
                priceId: priceId,
                paymentModeType: paymentModeType);

            var checkoutSessionUrl = await _paymentService.CreateCheckoutSession(checkoutSessionParams);

            return checkoutSessionUrl;
        }
    }
}
