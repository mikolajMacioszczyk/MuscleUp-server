using AutoMapper;
using Carnets.Application.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Application.Permissions.Queries;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.GympassTypes.Commands
{
    public record UpdateGympassTypeWithPermissionsCommand : IRequest<Result<GympassTypeWithPermissions>>
    {
        public GympassType GympassType { get; init; }

        public IEnumerable<string> ClassPermissions { get; init; }

        public IEnumerable<string> PerkPermissions { get; init; }
    }

    public class UpdateGympassTypeWithPermissionsCommandHandler : IRequestHandler<UpdateGympassTypeWithPermissionsCommand, Result<GympassTypeWithPermissions>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPaymentService _paymentService;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UpdateGympassTypeWithPermissionsCommandHandler(
            IGympassTypeRepository gympassTypeRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            IPaymentService paymentService,
            ISender mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _gympassTypeRepository = gympassTypeRepository;
            _paymentService = paymentService;
            _assignedPermissionRepository = assignedPermissionRepository;
            _mapper = mapper;
        }

        public async Task<Result<GympassTypeWithPermissions>> Handle(UpdateGympassTypeWithPermissionsCommand request, CancellationToken cancellationToken)
        {
            var getPermissionsQuery = new GetPermissionsByNamesQuery()
            {
                ClassPermissionsNames = request.ClassPermissions,
                PerkPermissionsNames = request.PerkPermissions
            };

            var allPermissionResult = await _mediator.Send(getPermissionsQuery);
            if (!allPermissionResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(allPermissionResult.Errors);
            }
            var (classPermissions, perkPermissions) = allPermissionResult.Value;

            var updateResult = await _gympassTypeRepository.UpdateGympassType(request.GympassType);

            if (!updateResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(updateResult.Errors);
            }

            var assignResult = await GympassTypeHelper.AssignAllGympassPermissions(classPermissions,
                perkPermissions, updateResult.Value, _mediator);

            if (!assignResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(assignResult.Errors);
            }
            await _paymentService.CreateProduct(updateResult.Value);

            await _gympassTypeRepository.SaveChangesAsync();
            await _assignedPermissionRepository.SaveChangesAsync();

            var gympassWithPermissions = GympassTypeHelper.MapToGympassTypeWithPermissions(updateResult.Value, 
                request.ClassPermissions, request.PerkPermissions, _mapper);
            return new Result<GympassTypeWithPermissions>(gympassWithPermissions);
        }
    }
}
