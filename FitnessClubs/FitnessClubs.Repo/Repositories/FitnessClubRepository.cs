using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class FitnessClubRepository : IFitnessClubRepository
    {
        private readonly FitnessClubsDbContext _context;

        public FitnessClubRepository(FitnessClubsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FitnessClub>> GetAll(bool asTracking)
        {
            IQueryable<FitnessClub> query = _context.FitnessClubs
                .Where(f => !f.IsDeleted);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<FitnessClub> GetById(string fitnessClubId, bool asTracking)
        {
            IQueryable<FitnessClub> query = _context.FitnessClubs
                .Where(f => !f.IsDeleted);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(f => f.FitnessClubId == fitnessClubId);
        }

        public async Task<IEnumerable<FitnessClub>> GetOwnerFitnessClubs(string ownerId, bool onlyActive, bool asTracking)
        {
            IQueryable<FitnessClub> query = _context.FitnessClubs
                .Where(f => f.OwnerId == ownerId);

            if (onlyActive)
            {
                query = query.Where(f => !f.IsDeleted);
            }

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Result<FitnessClub>> Create(FitnessClub fitnessClub)
        {
            if ((await GetByName(fitnessClub.FitnessClubName, false)) != null)
            {
                return new Result<FitnessClub>($"Fitness club with name {fitnessClub.FitnessClubName} already exists");
            }

            fitnessClub.FitnessClubId = Guid.NewGuid().ToString();

            await _context.FitnessClubs.AddAsync(fitnessClub);

            return new Result<FitnessClub>(fitnessClub);
        }

        public async Task<Result<bool>> Delete(string fitnessClubId)
        {
            var fitnessClubFromDb = await GetById(fitnessClubId, true);
            if (fitnessClubFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            fitnessClubFromDb.IsDeleted = true;

            return new Result<bool>(true);
        }

        private async Task<FitnessClub> GetByName(string fitnessClubName, bool asTracking)
        {
            IQueryable<FitnessClub> query = _context.FitnessClubs
                .Where(f => !f.IsDeleted);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(f => f.FitnessClubName == fitnessClubName);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
