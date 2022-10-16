using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class PerkPermissionRepository : PermissionRepositoryBase<PerkPermission>
    {
        public PerkPermissionRepository(CarnetsDbContext context) : base(context)
        { }

        protected override DbSet<PerkPermission> PermissionDbSet => _context.PerkPermissions;

        public override async Task<Result<PerkPermission>> CreatePermission(PerkPermission newPermission)
        {
            var existing = await GetPermissionByName(newPermission.PermissionName);

            if (existing != null)
            {
                return new Result<PerkPermission>(existing);
            }

            return await base.CreatePermission(newPermission);
        }

        public override async Task<Result<IEnumerable<PerkPermission>>> GetAllPermissionsByNames(IEnumerable<string> permissionNames, bool asTracking)
        {
            var query = PermissionDbSet.Where(c => permissionNames.Contains(c.PermissionName));

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var result = await query.ToListAsync();

            if (result.Count == permissionNames.Count())
            {
                return new Result<IEnumerable<PerkPermission>>(result);
            }

            var resultNames = result.Select(r => r.PermissionName).ToList();
            var notExisting = permissionNames.Where(n => !resultNames.Contains(n));

            return new Result<IEnumerable<PerkPermission>>(notExisting.Select(n => $"Permission with name \"{n}\" does not exists").ToArray());
        }

        private Task<PerkPermission> GetPermissionByName(string name)
        {
            return PermissionDbSet.FirstOrDefaultAsync(c => c.PermissionName.Equals(name));
        }
    }
}
