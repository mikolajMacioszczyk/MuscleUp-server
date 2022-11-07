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
                FitnessClubLogoUrl = SeedConsts.DefaultFitnessClubLogoUrl,
                OwnerId = SeedConsts.DefaultOwnerId
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

            // seed trainers employments
            var defaultTrainer1Employment = new TrainerEmployment()
            {
                EmploymentId = SeedConsts.DefaultTrainer1EmploymentId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClub = defaultFitnessClub,
                UserId = SeedConsts.DefaultTrainerId,
                EmployedFrom = DateTime.UtcNow,
                EmployedTo = null
            };

            var defaultTrainer2Employment = new TrainerEmployment()
            {
                EmploymentId = SeedConsts.DefaultTrainer2EmploymentId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClub = defaultFitnessClub,
                UserId = SeedConsts.DefaultTrainer2Id,
                EmployedFrom = DateTime.UtcNow,
                EmployedTo = null
            };

            var defaultTrainer3Employment = new TrainerEmployment()
            {
                EmploymentId = SeedConsts.DefaultTrainer3EmploymentId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                FitnessClub = defaultFitnessClub,
                UserId = SeedConsts.DefaultTrainer3Id,
                EmployedFrom = DateTime.UtcNow,
                EmployedTo = null
            };

            await context.TrainerEmployments.AddAsync(defaultTrainer1Employment);
            await context.TrainerEmployments.AddAsync(defaultTrainer2Employment);
            await context.TrainerEmployments.AddAsync(defaultTrainer3Employment);

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
