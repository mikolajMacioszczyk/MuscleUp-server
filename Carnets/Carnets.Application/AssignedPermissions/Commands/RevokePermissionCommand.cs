using Carnets.Application.Interfaces;
using Common.Models;
using MediatR;

namespace Carnets.Application.AssignedPermissions.Commands
{
    public record RevokePermissionCommand : IRequest<Result<bool>>
    {
        public string PermissionId { get; init; }
        public string FitnessClubId { get; init; }
        public string GympassTypeId { get; init; }
    }

    public class RevokePermissionCommandHandler : IRequestHandler<RevokePermissionCommand, Result<bool>>
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public RevokePermissionCommandHandler(IAssignedPermissionRepository assignedPermissionRepository)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public async Task<Result<bool>> Handle(RevokePermissionCommand request, CancellationToken cancellationToken)
        {
            var removeResult = await _assignedPermissionRepository.RemovePermission(
                request.PermissionId, request.GympassTypeId, request.FitnessClubId);

            if (removeResult.IsSuccess)
            {
                await _assignedPermissionRepository.SaveChangesAsync();
            }

            return removeResult;
        }
    }
}
