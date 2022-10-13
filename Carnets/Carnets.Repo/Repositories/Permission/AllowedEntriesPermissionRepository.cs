using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class AllowedEntriesPermissionRepository : PermissionRepositoryBase<AllowedEntriesPermission>
    {
        public AllowedEntriesPermissionRepository(CarnetsDbContext context) : base(context)
        { }

        protected override DbSet<AllowedEntriesPermission> PermissionDbSet => _context.AllowedEntriesPermissions;

        public override async Task<Result<IEnumerable<AllowedEntriesPermission>>> GetAllPermissionsByNames(IEnumerable<string> permissionNames, bool asTracking)
        {
            var query = PermissionDbSet.Where(c => permissionNames.Contains(c.PermissionId));

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var result = await query.ToListAsync();

            if (result.Count == permissionNames.Count())
            {
                return new Result<IEnumerable<AllowedEntriesPermission>>(result);
            }

            var resultNames = result.Select(r => r.PermissionId).ToList();
            var notExisting = permissionNames.Where(n => !resultNames.Contains(n));

            return new Result<IEnumerable<AllowedEntriesPermission>>(notExisting.Select(n => $"Permission with name {n} does not exists").ToArray());
        }
    }
}
