using Carnets.Application.GympassTypes.Queries;
using Carnets.Application.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.GympassTypes.Commands
{
    public record UpdateGympassTypeCommand : IRequest<Result<GympassTypeWithPermissions>>
    {
        public GympassType GympassType { get; init; }
    }

    public class UpdateGympassTypeCommandHandler : IRequestHandler<UpdateGympassTypeCommand, Result<GympassTypeWithPermissions>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPaymentService _paymentService;
        private readonly ISender _mediator;

        public UpdateGympassTypeCommandHandler(
            IGympassTypeRepository gympassTypeRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            IPaymentService paymentService,
            ISender mediator)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
            _paymentService = paymentService;
            _mediator = mediator;
        }

        public async Task<Result<GympassTypeWithPermissions>> Handle(UpdateGympassTypeCommand request, CancellationToken cancellationToken)
        {
            GympassTypeHelper.ValidateGympassIntervals(request.GympassType);

            var updateResult = await _gympassTypeRepository.UpdateGympassType(request.GympassType);

            if (updateResult.IsSuccess)
            {
                var linkedPermissionsResult = await _assignedPermissionRepository
                    .GetAllGympassPermissions(request.GympassType.GympassTypeId, true);

                if (!linkedPermissionsResult.IsSuccess)
                {
                    return new Result<GympassTypeWithPermissions>(linkedPermissionsResult.Errors);
                }

                foreach (var oldAssignedPermission in linkedPermissionsResult.Value)
                {
                    var newAssignedPermission = new AssignedPermission
                    {
                        Permission = oldAssignedPermission.Permission,
                        PermissionId = oldAssignedPermission.PermissionId,
                        GympassType = updateResult.Value,
                        GympassTypeId = updateResult.Value.GympassTypeId
                    };

                    var createResult = await _assignedPermissionRepository.CreateAssignedPermission(newAssignedPermission);

                    if (!createResult.IsSuccess)
                    {
                        return new Result<GympassTypeWithPermissions>(createResult.Errors);
                    }
                }

                await _paymentService.CreateProduct(updateResult.Value);

                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();

                var gympassWithPermissions = await _mediator.Send(new AssignPermissionsToGympassTypeQuery()
                {
                    GympassType = updateResult.Value
                });

                return new Result<GympassTypeWithPermissions>(gympassWithPermissions);
            }

            return new Result<GympassTypeWithPermissions>(updateResult.Errors);
        }
    }
}
