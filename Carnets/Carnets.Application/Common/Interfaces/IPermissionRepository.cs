using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Application.Interfaces
{
    public interface IPermissionRepository<TPermission> where TPermission : PermissionBase
    {
        Task<IEnumerable<TPermission>> GetAll(string fitnessClubId, bool asTracking);

        Task<TPermission> GetPermissionById(string permissionId, bool asTracking);

        Task<IEnumerable<TPermission>> GetPermissionByIds(string[] permissionIds, bool asTracking);

        Task<Result<IEnumerable<TPermission>>> GetAllPermissionsByNames(IEnumerable<string> permissionNames, bool asTracking);

        Task<Result<TPermission>> CreatePermission(TPermission newPermission);

        Task<Result<bool>> DeletePermission(string permissionId, string fitnessClubId);

        Task SaveChangesAsync();
    }
}
