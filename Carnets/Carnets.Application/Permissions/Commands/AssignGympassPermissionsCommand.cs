using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Permissions.Commands
{
    public record AssignGympassPermissionsCommand : IRequest<Result<GympassType>>
    {
        public IEnumerable<PermissionBase> Permissions { get; init; }
        public GympassType GympassType { get; init; }
    }

    public class AssignGympassPermissionsCommandHandler : IRequestHandler<AssignGympassPermissionsCommand, Result<GympassType>>
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public AssignGympassPermissionsCommandHandler(IAssignedPermissionRepository assignedPermissionRepository)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public async Task<Result<GympassType>> Handle(AssignGympassPermissionsCommand request, CancellationToken cancellationToken)
        {
            foreach (var permission in request.Permissions)
            {
                var assigement = new AssignedPermission()
                {
                    GympassTypeId = request.GympassType.GympassTypeId,
                    GympassType = request.GympassType,
                    PermissionId = permission.PermissionId,
                    Permission = permission
                };

                var assignResult = await _assignedPermissionRepository.CreateAssignedPermission(assigement);

                if (!assignResult.IsSuccess)
                {
                    return new Result<GympassType>(assignResult.Errors);
                }
            }

            return new Result<GympassType>(request.GympassType);
        }
    }
}
