using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
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

        public async Task<TPermission> GetPermissionById(string permissionId)
        {
            return await PermissionDbSet.FirstOrDefaultAsync(p => p.PermissionId == permissionId);
        }
    }
}
