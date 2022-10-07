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
            var isUnique = (
                await GetPermissionByName(newPermission.PermissionName)
                ) is null;

            if (!isUnique)
            {
                return new Result<ClassPermission>($"ClassPermission with name {newPermission.PermissionName} already exists");
            }

            return await base.CreatePermission(newPermission);
        }

        private Task<ClassPermission> GetPermissionByName(string name)
        {
            return _context.ClassPermissions.FirstOrDefaultAsync(c => c.PermissionName.Equals(name));
        }
    }
}
