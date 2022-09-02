using Carnets.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class TimePermissionEntryRepository : PermissionRepositoryBase<TimePermissionEntry>
    {
        public TimePermissionEntryRepository(CarnetsDbContext? context) : base(context)
        { }

        protected override DbSet<TimePermissionEntry> PermissionDbSet => _context.TimePermissionEntries;
    }
}
