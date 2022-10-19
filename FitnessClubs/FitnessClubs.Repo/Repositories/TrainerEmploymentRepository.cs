using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class TrainerEmploymentRepository : EmploymentRepositoryBase<TrainerEmployment>, IEmploymentRepository<TrainerEmployment>
    {
        public override DbSet<TrainerEmployment> Employments => _context.TrainerEmployments;

        public TrainerEmploymentRepository(FitnessClubsDbContext context) : base(context)
        { }
    }
}
