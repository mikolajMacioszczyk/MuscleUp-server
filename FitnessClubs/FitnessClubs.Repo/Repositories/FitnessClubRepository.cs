using Common.Models;
using FitnessClubs.Domain.Interfaces;
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

        public async Task<IEnumerable<FitnessClub>> GetAll()
        {
            return await _context.FitnessClubs.ToListAsync();
        }

        public async Task<FitnessClub> GetById(string fitnessClubId)
        {
            return await _context.FitnessClubs.FirstOrDefaultAsync(f => f.FitnessClubId == fitnessClubId);
        }

        public async Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId)
        {
            var workerEmployment = await _context.WorkerEmployments
                .Include(e => e.FitnessClub)
                .FirstOrDefaultAsync(w => w.UserId == workerId);

            if (workerEmployment is null)
            {
                return new Result<FitnessClub>(new string[] { Common.CommonConsts.NOT_FOUND, $"An employee with an id {workerId} is not employed by any fitness club" });
            }

            if (workerEmployment.FitnessClub is null)
            {
                throw new ArgumentNullException(nameof(workerEmployment.FitnessClub));
            }

            return new Result<FitnessClub>(workerEmployment.FitnessClub);
        }

        public async Task<Result<FitnessClub>> Create(FitnessClub fitnessClub)
        {
            if ((await GetByName(fitnessClub.FitnessClubName)) != null)
            {
                return new Result<FitnessClub>($"Fitness club with name {fitnessClub.FitnessClubName} already exists");
            }

            fitnessClub.FitnessClubId = Guid.NewGuid().ToString();

            await _context.FitnessClubs.AddAsync(fitnessClub);
            await _context.SaveChangesAsync();

            return new Result<FitnessClub>(fitnessClub);
        }

        public async Task<Result<bool>> Delete(string fitnessClubId)
        {
            var fitnessClubFromDb = await GetById(fitnessClubId);
            if (fitnessClubFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }
            // TODO: What if FitnessClub have carnets?
            _context.FitnessClubs.Remove(fitnessClubFromDb);
            await _context.SaveChangesAsync();

            return new Result<bool>(true);
        }

        private async Task<FitnessClub> GetByName(string fitnessClubName)
        {
            return await _context.FitnessClubs.FirstOrDefaultAsync(f => f.FitnessClubName == fitnessClubName);
        }
    }
}
