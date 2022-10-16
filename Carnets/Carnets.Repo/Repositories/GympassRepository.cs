using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class GympassRepository : IGympassRepository
    {
        private readonly CarnetsDbContext _context;
        private readonly IGympassTypeRepository _gympassTypeRepository;

        public GympassRepository(CarnetsDbContext context, IGympassTypeRepository gympassTypeRepository)
        {
            _context = context;
            _gympassTypeRepository = gympassTypeRepository;
        }

        public async Task<IEnumerable<Gympass>> GetAll(bool asTracking)
        {
            IQueryable<Gympass> query = _context.Gympasses.Include(g => g.GympassType);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Gympass>> GetAllFromFitnessClub(string fitnessClubId, bool asTracking)
        {
            var interestedGympassTypesIds = (await _gympassTypeRepository.GetAllGympassTypes(fitnessClubId, false, 0, int.MaxValue, asTracking))
                .Select(g => g.GympassTypeId);

            var query = _context.Gympasses
                .Include(g => g.GympassType)
                .Where(g => interestedGympassTypesIds.Contains(g.GympassType.GympassTypeId));

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Gympass>> GetAllForMember(string memberId, bool asTracking)
        {
            IQueryable<Gympass> query = _context.Gympasses
                .Where(g => g.UserId == memberId)
                .Include(g => g.GympassType);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Gympass> GetById(string gympassId, bool asTracking)
        {
            IQueryable<Gympass> query = _context.Gympasses
            .Include(g => g.GympassType);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(g => g.GympassId == gympassId);
        }

        public async Task<Result<Gympass>> CreateGympass(string gympassTypeId, Gympass created)
        {
            var gympassType = await _gympassTypeRepository.GetGympassTypeById(gympassTypeId, true);

            if (gympassType is null)
            {
                return new Result<Gympass>($"Gymmpass type with id {gympassTypeId} not found");
            }

            created.GympassType = gympassType;
            created.RemainingValidityPeriodInSeconds = gympassType.ValidityPeriodInSeconds;
            created.RemainingEntries = gympassType.AllowedEntries;

            await _context.Gympasses.AddAsync(created);

            return new Result<Gympass>(created);
        }

        public Task<Result<Gympass>> UpdateGympass(Gympass updated)
        {
            if (updated == null)
            {
                throw new ArgumentException(nameof(updated));
            }

            _context.Entry(updated).State = EntityState.Modified;

            return Task.FromResult(new Result<Gympass>(updated));
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
