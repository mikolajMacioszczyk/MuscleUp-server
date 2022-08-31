using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo
{
    public class FitnessClubsDbContext : DbContext
    {
        public FitnessClubsDbContext(DbContextOptions<FitnessClubsDbContext> options) : base(options)
        { }

        public DbSet<FitnessClub> FitnessClubs { get; set; }

        public DbSet<TrainerEmployment> TrainerEmployments { get; set; }

        public DbSet<WorkerEmployment> WorkerEmployments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainerEmployment>()
                .HasKey(t => new { t.UserId, t.FitnessClubId });

            modelBuilder.Entity<WorkerEmployment>()
                .HasKey(w => new { w.UserId, w.FitnessClubId });

            modelBuilder.Entity<FitnessClub>()
                .HasIndex(f => f.FitnessClubName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
