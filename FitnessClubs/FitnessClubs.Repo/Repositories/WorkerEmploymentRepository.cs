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

        public async Task<WorkerEmployment> GetWorkerEmploymentById(string workerEmploymentId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments;

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(w => w.WorkerEmploymentId == workerEmploymentId);
        }

        public async Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId, bool includeInactive, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments
                .Where(w => w.FitnessClubId == fitnessClubId)
                .Include(w => w.FitnessClub);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            IEnumerable<WorkerEmployment> result = await query.ToListAsync();

            if (!includeInactive)
            {
                result = result.Where(w => !w.EmployedTo.HasValue);
            }

            return result;
        }

        public async Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments
                .Include(w => w.FitnessClub)
                .Where(w => w.UserId == workerId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var workerEmployments = await query.ToListAsync();
            var activeEmployment = workerEmployments.FirstOrDefault(e => e.IsActive);

            if (activeEmployment is null)
            {
                return new Result<FitnessClub>(new string[] { Common.CommonConsts.NOT_FOUND, $"An employee with an id {workerId} is not employed by any fitness club" });
            }

            if (activeEmployment.FitnessClub is null)
            {
                throw new ArgumentNullException(nameof(activeEmployment.FitnessClub));
            }

            return new Result<FitnessClub>(activeEmployment.FitnessClub);
        }

        public async Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment)
        {
            var existingEmployment = await GetActiveWorkerEmployment(workerEmployment.FitnessClubId, workerEmployment.UserId, false);

            if (existingEmployment != null)
            {
                return new Result<WorkerEmployment>("Employment already exists");
            }

            workerEmployment.WorkerEmploymentId = Guid.NewGuid().ToString();
            workerEmployment.EmployedFrom = DateTime.UtcNow;

            await _context.WorkerEmployments.AddAsync(workerEmployment);

            return new Result<WorkerEmployment>(workerEmployment);
        }

        public async Task<Result<WorkerEmployment>> TerminateWorkerEmployment(string workerEmploymentId)
        {
            var employment = await GetWorkerEmploymentById(workerEmploymentId, true);

            if (employment is null)
            {
                return new Result<WorkerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            if (!employment.IsActive)
            {
                return new Result<WorkerEmployment>("Cannot terminate inactive employment");
            }

            employment.EmployedTo = DateTime.UtcNow;

            return new Result<WorkerEmployment>(employment);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        private async Task<WorkerEmployment> GetActiveWorkerEmployment(string fitnessClubId, string workerId, bool asTracking)
        {
            IQueryable<WorkerEmployment> query = _context.WorkerEmployments
                .Where(w => w.FitnessClubId == fitnessClubId && w.UserId == workerId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var result = await query.ToListAsync();

            return result.FirstOrDefault(r => r.IsActive);
        }
    }
}
