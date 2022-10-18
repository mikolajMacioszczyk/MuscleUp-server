using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Application.Interfaces
{
    public interface IAssignedPermissionRepository
    {
        Task<AssignedPermission> GetAssignedPermissionById(string gympassTypeId, string permissionId, bool asTracking);

        Task<IEnumerable<AssignedPermission>> GetAllByPermission(string permissionId, bool asTracking);

        Task<Result<IEnumerable<AssignedPermission>>> GetAllGympassPermissions(string gympassTypeId, bool asTracking);

        Task<Result<AssignedPermission>> CreateAssignedPermission(AssignedPermission assignedPermission);

        Task<Result<bool>> RemovePermission(string permissionId, string gympassTypeId, string fitnessClubId);

        Task SaveChangesAsync();
    }
}
