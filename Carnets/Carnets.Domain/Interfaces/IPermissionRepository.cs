using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IPermissionRepository<TPermission> where TPermission : PermissionBase
    {
        Task<IEnumerable<TPermission>> GetAll();

        Task<TPermission> GetPermissionById(string permissionId);

        Task<Result<TPermission>> CreatePermission(TPermission newPermission);

        Task<Result<bool>> DeletePermission(string permissionId);
    }
}
