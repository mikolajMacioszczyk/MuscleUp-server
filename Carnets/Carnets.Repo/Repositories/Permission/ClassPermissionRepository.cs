using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class ClassPermissionRepository : PermissionRepositoryBase<ClassPermission>
    {
        public ClassPermissionRepository(CarnetsDbContext context) : base(context)
        { }

        protected override DbSet<ClassPermission> PermissionDbSet => _context.ClassPermissions;

        public override async Task<Result<ClassPermission>> CreatePermission(ClassPermission newPermission)
        {
            var existing = await GetPermissionByName(newPermission.PermissionName);

            if (existing != null)
            {
                return new Result<ClassPermission>(existing);
            }

            return await base.CreatePermission(newPermission);
        }

        public override async Task<Result<IEnumerable<ClassPermission>>> GetAllPermissionsByNames(IEnumerable<string> permissionNames, bool asTracking)
        {
            var query = PermissionDbSet.Where(c => permissionNames.Contains(c.PermissionName));

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var result = await query.ToListAsync();

            if (result.Count == permissionNames.Count())
            {
                return new Result<IEnumerable<ClassPermission>>(result);
            }

            var resultNames = result.Select(r => r.PermissionName).ToList();
            var notExisting = permissionNames.Where(n => !resultNames.Contains(n));

            return new Result<IEnumerable<ClassPermission>>(notExisting.Select(n => $"Permission with name \"{n}\" does not exists").ToArray());
        }

        private Task<ClassPermission> GetPermissionByName(string name)
        {
            return _context.ClassPermissions.FirstOrDefaultAsync(c => c.PermissionName.Equals(name));
        }
    }
}
