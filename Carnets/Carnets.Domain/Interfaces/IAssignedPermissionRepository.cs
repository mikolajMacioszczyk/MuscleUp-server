using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IAssignedPermissionRepository
    {
        Task<Result<IEnumerable<PermissionBase>>> GetAllGympassPermissions(string gympassTypeId);

        Task<Result<AssignedPermission>> GrantPermission(AssignedPermission grantRequest, string fitnessClubId);

        Task<Result<bool>> RevokePermission(string permissionId, string fitnessClubId, string gympassTypeId);

        Task<Result<bool>> RemovePermissionWithAllAssigements(string permissionId, string fitnessClubId);
    }
}
