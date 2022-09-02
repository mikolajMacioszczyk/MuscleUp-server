using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public abstract class PermissionRepositoryBase<TPermission> : IPermissionRepository<TPermission>
        where TPermission : PermissionBase
    {
        protected readonly CarnetsDbContext _context;

        protected PermissionRepositoryBase(CarnetsDbContext? context)
        {
            _context = context;
        }

        protected abstract DbSet<TPermission> PermissionDbSet { get; }

        public async Task<Result<bool>> DeletePermission(string permissionId)
        {
            var permissionFromDb = await PermissionDbSet.FirstOrDefaultAsync(x => x.PermissionId == permissionId);

            if (permissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var linkedAssignedPermissions = await _context.AssignedPermissions
                .Where(a => a.PermissionId == permissionId)
                .ToListAsync();

            if (linkedAssignedPermissions.Any())
            {
                return new Result<bool>("Operation not permitted. Cannot delete Permission with linked AssignedPermissions");
            }

            PermissionDbSet.Remove(permissionFromDb);
            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }

        public async Task<TPermission> GetPermissionById(string permissionId)
        {
            return await PermissionDbSet.FirstOrDefaultAsync(p => p.PermissionId == permissionId);
        }
    }
}
