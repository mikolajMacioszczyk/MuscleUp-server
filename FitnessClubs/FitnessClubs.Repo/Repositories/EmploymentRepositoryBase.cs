using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public abstract class EmploymentRepositoryBase<TEmployment> : IEmploymentRepository<TEmployment>
        where TEmployment : EmploymentBase
    {
        protected readonly FitnessClubsDbContext _context;

        protected EmploymentRepositoryBase(FitnessClubsDbContext context)
        {
            _context = context;
        }

        public abstract DbSet<TEmployment> Employments { get; }

        public async Task<TEmployment> GetEmploymentById(string employmentId, bool asTracking)
        {
            IQueryable<TEmployment> query = Employments;

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(w => w.EmploymentId == employmentId);
        }

        public async Task<IEnumerable<TEmployment>> GetAllEmployments(string fitnessClubId, bool includeInactive, bool asTracking)
        {
            IQueryable<TEmployment> query = Employments
                .Where(w => w.FitnessClubId == fitnessClubId)
                .Include(w => w.FitnessClub);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            IEnumerable<TEmployment> result = await query.ToListAsync();

            if (!includeInactive)
            {
                result = result.Where(w => !w.EmployedTo.HasValue);
            }

            return result;
        }

        public async Task<Result<FitnessClub>> GetFitnessClubOfEmployee(string workerId, bool asTracking)
        {
            IQueryable<TEmployment> query = Employments
                .Include(w => w.FitnessClub)
                .Where(w => w.UserId == workerId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var employments = await query.ToListAsync();
            var activeEmployment = employments.FirstOrDefault(e => e.IsActive);

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

        public async Task<IEnumerable<FitnessClub>> GetAllFitnessClubsOfEmployee(
            string employeeId, 
            bool onlyActive, 
            bool asTracking)
        {
            IQueryable<TEmployment> query = Employments
                .Include(w => w.FitnessClub)
                .Where(w => w.UserId == employeeId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var employments = await query.ToListAsync();

            var resultEmployments = onlyActive ? employments.Where(e => e.IsActive) : employments;

            return resultEmployments.Select(e => e.FitnessClub);
        }

        public async Task<Result<TEmployment>> CreateEmployment(TEmployment employment)
        {
            var existingEmployment = await GetActiveEmployment(employment.FitnessClubId, employment.UserId, false);

            if (existingEmployment != null)
            {
                return new Result<TEmployment>("Employment already exists");
            }

            employment.EmploymentId = Guid.NewGuid().ToString();
            employment.EmployedFrom = DateTime.UtcNow;

            await Employments.AddAsync(employment);

            return new Result<TEmployment>(employment);
        }

        public async Task<Result<TEmployment>> TerminateEmployment(string employmentId)
        {
            var employment = await GetEmploymentById(employmentId, true);

            if (employment is null)
            {
                return new Result<TEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            if (!employment.IsActive)
            {
                return new Result<TEmployment>("Cannot terminate inactive employment");
            }

            employment.EmployedTo = DateTime.UtcNow;

            return new Result<TEmployment>(employment);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        protected async Task<TEmployment> GetActiveEmployment(string fitnessClubId, string employeeId, bool asTracking)
        {
            IQueryable<TEmployment> query = Employments
                .Where(w => w.FitnessClubId == fitnessClubId && w.UserId == employeeId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            var result = await query.ToListAsync();

            return result.FirstOrDefault(r => r.IsActive);
        }
    }
}
