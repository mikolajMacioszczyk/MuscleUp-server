using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.Interfaces
{
    public interface IEmploymentRepository<TEmployment>
        where TEmployment : EmploymentBase
    {
        Task<TEmployment> GetEmploymentById(string employmentId, bool asTracking);

        Task<IEnumerable<TEmployment>> GetAllEmployments(string fitnessClubId, bool includeInactive, bool asTracking);

        Task<Result<FitnessClub>> GetFitnessClubOfEmployee(string employeeId, bool asTracking);

        Task<IEnumerable<FitnessClub>> GetAllFitnessClubsOfEmployee(string employeeId, bool onlyActive, bool asTracking);

        Task<Result<TEmployment>> CreateEmployment(TEmployment employment);

        Task<Result<TEmployment>> TerminateEmployment(string employmentId);

        Task SaveChangesAsync();
    }
}
