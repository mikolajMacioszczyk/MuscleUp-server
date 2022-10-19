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
            modelBuilder.Entity<FitnessClub>()
                .HasIndex(f => f.FitnessClubName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
