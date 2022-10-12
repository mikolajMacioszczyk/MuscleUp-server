using Common.Consts;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo
{
    public static class FitnessClubsDbContextSeed
    {
        public static async Task SeedDefaultFitnessClubsDataAsync(FitnessClubsDbContext context)
        {
            if (await context.FitnessClubs.AnyAsync())
            {
                return;
            }

            // seed fitness clubs
            var defaultFitnessClub = new FitnessClub()
            {
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClubName = "Default Fitness Club",
                Address = "Default address" 
            };

            await context.FitnessClubs.AddAsync(defaultFitnessClub);

            // seed worker employments
            var defaultWorkerEmployment = new WorkerEmployment()
            {
                WorkerEmploymentId = SeedConsts.DefaultWorkerEmploymentId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClub = defaultFitnessClub,
                UserId = SeedConsts.DefaultWorkerId,
                EmployedFrom = DateTime.UtcNow,
                EmployedTo = null
            };

            await context.WorkerEmployments.AddAsync(defaultWorkerEmployment);

            await context.SaveChangesAsync();
        }
    }
}
