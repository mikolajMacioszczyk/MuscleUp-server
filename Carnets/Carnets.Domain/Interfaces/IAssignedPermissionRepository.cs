using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IAssignedPermissionRepository
    {
        Task<Result<IEnumerable<AssignedPermission>>> GetAllGympassPermissions(string gympassTypeId);

        Task<Result<AssignedPermission>> GrantPermission(AssignedPermission grantRequest);

        Task<Result<bool>> RevokePermission(string permissionId, string gympassTypeId);

        Task<Result<bool>> RemovePermissionWithAllAssigements(string permissionId);
    }
}
