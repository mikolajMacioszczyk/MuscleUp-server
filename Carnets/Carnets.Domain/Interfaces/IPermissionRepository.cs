using Carnets.Domain.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IPermissionRepository<TPermission> where TPermission : PermissionBase
    {
        Task<TPermission> GetPermissionById(string permissionId);
    }
}
