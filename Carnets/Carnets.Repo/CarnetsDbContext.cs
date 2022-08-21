using Carnets.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo
{
    public class CarnetsDbContext : DbContext
    {
        public CarnetsDbContext(DbContextOptions<CarnetsDbContext> options) : base(options)
        { }
        
        public DbSet<Gympass> Gympasses { get; set; }
        public DbSet<GympassType> GympassTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AllowedEntriesPermission> AllowedEntriesPermissions { get; set; }
        public DbSet<ClassPermission> ClassPermissions { get; set; }
        public DbSet<TimePermissionEntry> TimePermissionEntries { get; set; }
        public DbSet<AssignedPermission> AssignedPermissions { get; set; }
    }
}
