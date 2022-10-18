using Carnets.Application.Interfaces;
using Common.Models;
using MediatR;

namespace Carnets.Application.GympassTypes.Commands
{
    public record DeleteGympassTypeCommand : IRequest<Result<bool>>
    {
        public string GympassTypeId { get; init; }
    }

    public class DeleteGympassTypeCommandHandler : IRequestHandler<DeleteGympassTypeCommand, Result<bool>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPaymentService _paymentService;

        public DeleteGympassTypeCommandHandler(
            IGympassTypeRepository gympassTypeRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            IPaymentService paymentService)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
            _paymentService = paymentService;
        }

        public async Task<Result<bool>> Handle(DeleteGympassTypeCommand request, CancellationToken cancellationToken)
        {
            var deleteResult = await _gympassTypeRepository.DeleteGympassType(request.GympassTypeId);

            if (deleteResult.IsSuccess)
            {
                var linkedPermissionsResult = await _assignedPermissionRepository
                    .GetAllGympassPermissions(request.GympassTypeId, true);

                if (!linkedPermissionsResult.IsSuccess)
                {
                    return new Result<bool>(linkedPermissionsResult.ErrorCombined);
                }

                foreach (var assignedPermission in linkedPermissionsResult.Value)
                {
                    var deleteAssignedResult = await _assignedPermissionRepository.RemovePermission(assignedPermission.PermissionId,
                        assignedPermission.GympassTypeId, assignedPermission.Permission?.FitnessClubId);

                    if (!deleteAssignedResult.IsSuccess)
                    {
                        return deleteAssignedResult;
                    }
                }
                await _paymentService.DeleteProduct(request.GympassTypeId);

                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();
            }

            return deleteResult;
        }
    }
}
