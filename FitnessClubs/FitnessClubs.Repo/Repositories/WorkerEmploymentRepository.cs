using Common.Models;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class WorkerEmploymentRepository : IWorkerEmploymentRepository
    {
        private readonly FitnessClubsDbContext _context;

        public WorkerEmploymentRepository(FitnessClubsDbContext context)
        {
            _context = context;
        }

        public async Task<WorkerEmployment> GetWorkerEmploymentById(string fitnessClubId, string workerId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments;

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(w => w.FitnessClubId == fitnessClubId && w.UserId == workerId);
        }

        public async Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments
                .Where(w => w.FitnessClubId == fitnessClubId)
                .Include(w => w.FitnessClub);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments
                .Include(w => w.FitnessClub);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var workerEmployment = await query.FirstOrDefaultAsync(w => w.UserId == workerId);

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

        public async Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment)
        {
            var existingEmployment = await GetWorkerEmploymentById(workerEmployment.FitnessClubId, workerEmployment.UserId, false);

            if (existingEmployment != null)
            {
                return new Result<WorkerEmployment>("Employment already exists");
            }

            await _context.WorkerEmployments.AddAsync(workerEmployment);

            return new Result<WorkerEmployment>(workerEmployment);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
