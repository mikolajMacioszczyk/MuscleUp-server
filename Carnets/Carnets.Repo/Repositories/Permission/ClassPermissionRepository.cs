using Carnets.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class ClassPermissionRepository : PermissionRepositoryBase<ClassPermission>
    {
        public ClassPermissionRepository(CarnetsDbContext? context) : base(context)
        { }

        protected override DbSet<ClassPermission> PermissionDbSet => _context.ClassPermissions;
    }
}
