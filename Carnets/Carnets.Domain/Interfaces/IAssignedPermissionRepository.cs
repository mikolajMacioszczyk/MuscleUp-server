using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IAssignedPermissionRepository
    {
        Task<Result<AssignedPermission>> GrantPermission(AssignedPermission grantRequest);
    }
}
