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
                Address = "Default address",
                FitnessClubLogoUrl = SeedConsts.DefaultFitnessClubLogoUrl
            };

            await context.FitnessClubs.AddAsync(defaultFitnessClub);

            // seed worker employments
            var defaultWorkerEmployment = new WorkerEmployment()
            {
                EmploymentId = SeedConsts.DefaultWorkerEmploymentId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClub = defaultFitnessClub,
                UserId = SeedConsts.DefaultWorkerId,
                EmployedFrom = DateTime.UtcNow,
                EmployedTo = null
            };

            await context.WorkerEmployments.AddAsync(defaultWorkerEmployment);

            // seed memberships
            var defaultMembership = new Membership()
            {
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                MemberId = SeedConsts.DefaultMemberId,
                JoiningDate = DateTime.UtcNow
            };

            await context.Memberships.AddAsync(defaultMembership);

            await context.SaveChangesAsync();
        }
    }
}
