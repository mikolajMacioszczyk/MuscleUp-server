using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class WorkerEmploymentRepository : EmploymentRepositoryBase<WorkerEmployment>, IEmploymentRepository<WorkerEmployment>
    {
        public override DbSet<WorkerEmployment> Employments => _context.WorkerEmployments;

        public WorkerEmploymentRepository(FitnessClubsDbContext context) : base(context)
        { }
    }
}
