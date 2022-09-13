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

        public async Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments()
        {
            return await _context.WorkerEmployments
                .Include(w => w.FitnessClub)
                .ToListAsync();
        }

        public async Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment)
        {
            // TODO: Validate UserId
            var fitnessClubFromDb = await _context.FitnessClubs
                .FirstOrDefaultAsync(f => f.FitnessClubId == workerEmployment.FitnessClubId);

            if (fitnessClubFromDb is null)
            {
                return new Result<WorkerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            var existingEmployment = await _context.WorkerEmployments
                .FirstOrDefaultAsync(w => w.FitnessClubId == workerEmployment.FitnessClubId && w.UserId == workerEmployment.UserId);

            if (existingEmployment != null)
            {
                return new Result<WorkerEmployment>("Employment already exists");
            }

            workerEmployment.FitnessClub = fitnessClubFromDb;

            await _context.WorkerEmployments.AddAsync(workerEmployment);
            await _context.SaveChangesAsync();

            return new Result<WorkerEmployment>(workerEmployment);
        }
    }
}
