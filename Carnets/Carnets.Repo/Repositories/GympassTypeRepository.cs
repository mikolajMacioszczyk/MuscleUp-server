using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class GympassTypeRepository : IGympassTypeRepository
    {
        private readonly CarnetsDbContext _context;

        public GympassTypeRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public async Task<GympassType?> GetGympassById(string gympassId)
        {
            if (string.IsNullOrWhiteSpace(gympassId))
            {
                return null;
            }

            return await _context.GympassTypes.FirstOrDefaultAsync(g => g.GympassTypeId == gympassId);
        }

        private async Task<GympassType?> GetGympassTypeByNameAndFitnessClub(string name, string fitnessClubId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(fitnessClubId))
            {
                return null;
            }

            return await _context.GympassTypes.FirstOrDefaultAsync(g => 
                g.GympassTypeName == name && g.FitnessClubId == fitnessClubId);
        }

        public async Task<Result<GympassType>> CreateGympassType(GympassType gympassType)
        {
            var isUnique = (
                await GetGympassTypeByNameAndFitnessClub(gympassType.GympassTypeName, gympassType.FitnessClubId)
                ) is null;
            
            if (!isUnique)
            {
                return new Result<GympassType>($"GympassType with name {gympassType.GympassTypeName} already exists");
            }

            await _context.GympassTypes.AddAsync(gympassType);
            await _context.SaveChangesAsync();

            return new Result<GympassType>(gympassType);
        }

        public async Task<Result<GympassType>> UpdateGympassType(GympassType gympassType)
        {
            var gympassFromDb = await GetGympassById(gympassType.GympassTypeId);
            if (gympassFromDb is null)
            {
                return new Result<GympassType>(Common.CommonConsts.NOT_FOUND);
            }

            gympassFromDb.Price = gympassType.Price;
            gympassFromDb.ValidityPeriodInSeconds = gympassType.ValidityPeriodInSeconds;
            // new version
            return
        }
    }
}
