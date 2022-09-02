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

        private async Task<GympassType?> GetActiveGympassTypeByNameAndFitnessClub(string name, string fitnessClubId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(fitnessClubId))
            {
                return null;
            }

            return await _context.GympassTypes
                .Where(g => g.GympassTypeName == name && g.FitnessClubId == fitnessClubId)
                .FirstOrDefaultAsync(g => g.IsActive);
        }

        public async Task<Result<GympassType>> CreateGympassType(GympassType gympassType)
        {
            var isUnique = (
                await GetActiveGympassTypeByNameAndFitnessClub(gympassType.GympassTypeName, gympassType.FitnessClubId)
                ) is null;
            
            if (!isUnique)
            {
                return new Result<GympassType>($"GympassType with name {gympassType.GympassTypeName} already exists");
            }

            gympassType.GympassTypeId = Guid.NewGuid().ToString();
            gympassType.IsActive = true;
            gympassType.Version = 1;

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

            if (!gympassType.IsActive)
            {
                return new Result<GympassType>("Operation not allowed. An inactive gympass type cannot be changed.");
            }

            var updated = new GympassType()
            {
                GympassTypeId = Guid.NewGuid().ToString(),
                GympassTypeName = gympassFromDb.GympassTypeName,
                FitnessClubId = gympassFromDb.FitnessClubId,
                IsActive = true,
                Version = gympassFromDb.Version + 1
            };

            // set previous version inactive
            gympassType.IsActive = false;

            // update domain properties
            updated.Price = gympassType.Price;
            updated.ValidityPeriodInSeconds = gympassType.ValidityPeriodInSeconds;

            await _context.GympassTypes.AddAsync(updated);
            await _context.SaveChangesAsync();

            return new Result<GympassType>(updated);
        }

        public async Task<IEnumerable<GympassType>> GetAllActiveGympassTypes()
        {
            return await _context.GympassTypes.Where(g => g.IsActive).ToListAsync();
        }
    }
}
