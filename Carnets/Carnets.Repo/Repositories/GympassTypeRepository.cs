using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Carnets.Repo.Repositories
{
    public class GympassTypeRepository : IGympassTypeRepository
    {
        private readonly CarnetsDbContext _context;

        public GympassTypeRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public async Task<GympassType> GetGympassTypeById(string gympassId, bool asTracking)
        {
            IQueryable<GympassType> query = _context.GympassTypes;

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(g => g.GympassTypeId == gympassId);
        }

        public async Task<IEnumerable<GympassType>> GetAllGympassTypes<T>(
            string fitnessClubId, 
            bool onlyActive,
            int pageNumber, 
            int pageSize,
            bool asTracking,
            Expression<Func<GympassType, T>> orderBy)
        {
            var query = _context.GympassTypes
                .Where(g => g.FitnessClubId == fitnessClubId);

            if (onlyActive)
            {
                query = query.Where(g => g.IsActive);
            }

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            query = query.Skip(pageNumber * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        private async Task<GympassType> GetActiveGympassTypeByNameAndFitnessClub(string name, string fitnessClubId, bool asTracking)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(fitnessClubId))
            {
                return null;
            }

            var query = _context.GympassTypes
                .Where(g => g.GympassTypeName == name && g.FitnessClubId == fitnessClubId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(g => g.IsActive);
        }

        public async Task<Result<GympassType>> CreateGympassType(GympassType gympassType)
        {
            var isUnique = (
                await GetActiveGympassTypeByNameAndFitnessClub(gympassType.GympassTypeName, gympassType.FitnessClubId, true)
                ) is null;
            
            if (!isUnique)
            {
                return new Result<GympassType>($"GympassType with name {gympassType.GympassTypeName} already exists");
            }

            gympassType.GympassTypeId = Guid.NewGuid().ToString();
            gympassType.IsActive = true;
            gympassType.Version = 1;

            await _context.GympassTypes.AddAsync(gympassType);

            return new Result<GympassType>(gympassType);
        }

        public async Task<Result<GympassType>> UpdateGympassType(GympassType gympassType)
        {
            var gympassFromDb = await GetGympassTypeById(gympassType.GympassTypeId, true);
            if (gympassFromDb is null)
            {
                return new Result<GympassType>(Common.CommonConsts.NOT_FOUND);
            }

            if (!gympassFromDb.IsActive)
            {
                return new Result<GympassType>("Operation not permitted. An inactive gympass type cannot be changed.");
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
            gympassFromDb.IsActive = false;

            // update domain properties
            updated.Price = gympassType.Price;
            updated.Description = gympassType.Description;
            updated.EnableEntryFromInMinutes = gympassType.EnableEntryFromInMinutes;
            updated.EnableEntryToInMinutes = gympassType.EnableEntryToInMinutes;
            updated.Interval = gympassType.Interval;
            updated.IntervalCount = gympassType.IntervalCount;
            updated.AllowedEntries = gympassType.AllowedEntries;
            updated.ValidationType = gympassType.ValidationType;

            await _context.GympassTypes.AddAsync(updated);

            return new Result<GympassType>(updated);
        }

        public async Task<Result<bool>> DeleteGympassType(string gympassTypeId)
        {
            var gympassFromDb = await GetGympassTypeById(gympassTypeId, true);
            if (gympassFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var linkedGympasses = await GetAllGympassesWithType(gympassTypeId, false);

            if (linkedGympasses.Any())
            {
                return new Result<bool>("Operation not permitted. Cannot delete GympassType with linked Gympasses");
            }

            _context.GympassTypes.Remove(gympassFromDb);

            return new Result<bool>(true);
        }

        private async Task<IEnumerable<Gympass>> GetAllGympassesWithType(string gympassTypeId, bool asTracking)
        {
            var query = _context.Gympasses.Include(g => g.GympassType)
                .Where(g => g.GympassType.GympassTypeId == gympassTypeId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
