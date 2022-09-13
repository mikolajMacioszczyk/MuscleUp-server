using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IPermissionRepository<TPermission> where TPermission : PermissionBase
    {
        Task<IEnumerable<TPermission>> GetAll(string fitnessClubId);

        Task<TPermission> GetPermissionById(string permissionId, string fitnessClubId);

        Task<Result<TPermission>> CreatePermission(TPermission newPermission);

        Task<Result<bool>> DeletePermission(string permissionId, string fitnessClubId);
    }
}
