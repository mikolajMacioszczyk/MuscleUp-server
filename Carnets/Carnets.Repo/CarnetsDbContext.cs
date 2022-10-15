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
        public DbSet<ClassPermission> ClassPermissions { get; set; }
        public DbSet<AssignedPermission> AssignedPermissions { get; set; }
        public DbSet<PerkPermission> PerkPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignedPermission>()
                .HasKey(a => new { a.GympassTypeId, a.PermissionId });

            modelBuilder.Entity<ClassPermission>()
                .HasIndex(c => c.PermissionName)
                .IsUnique();

            modelBuilder.Entity<Subscription>()
                .HasIndex(s => s.StripeCustomerId)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
