using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Commands
{
    public record DeletePermissionCommand<TPermission> : IRequest<Result<bool>>
        where TPermission : PermissionBase
    {
        public string PermissionId { get; init; }
        public string FitnessClubId { get; init; }
    }

    public class DeletePermissionCommandHandler<TPermission> : IRequestHandler<DeletePermissionCommand<TPermission>, Result<bool>>
        where TPermission : PermissionBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public DeletePermissionCommandHandler(
            IPermissionRepository<TPermission> permissionRepository, 
            IAssignedPermissionRepository assignedPermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public async Task<Result<bool>> Handle(DeletePermissionCommand<TPermission> request, CancellationToken cancellationToken)
        {
            var result = await _permissionRepository.DeletePermission(request.PermissionId, request.FitnessClubId);

            if (result.IsSuccess)
            {
                var connectedPermissions = await _assignedPermissionRepository
                    .GetAllByPermission(request.PermissionId, true);

                // TODO: if someone uses this permission, broadcast message

                foreach (var assignedPermission in connectedPermissions)
                {
                    var removeAssignedPermissionResult = await _assignedPermissionRepository
                        .RemovePermission(assignedPermission.PermissionId, assignedPermission.GympassTypeId, request.FitnessClubId);

                    if (!removeAssignedPermissionResult.IsSuccess)
                    {
                        return removeAssignedPermissionResult;
                    }
                }

                await _assignedPermissionRepository.SaveChangesAsync();
                await _permissionRepository.SaveChangesAsync();
            }

            return result;
        }
    }
}
