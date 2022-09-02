using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IPermissionRepository<TPermission> where TPermission : PermissionBase
    {
        Task<TPermission> GetPermissionById(string permissionId);

        Task<Result<bool>> DeletePermission(string permissionId);
    }
}
