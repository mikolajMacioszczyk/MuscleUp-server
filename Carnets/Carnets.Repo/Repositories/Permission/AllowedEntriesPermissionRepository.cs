using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class AllowedEntriesPermissionRepository : PermissionRepositoryBase<AllowedEntriesPermission>
    {
        public AllowedEntriesPermissionRepository(CarnetsDbContext? context) : base(context)
        { }

        protected override DbSet<AllowedEntriesPermission> PermissionDbSet => _context.AllowedEntriesPermissions;
    }
}
