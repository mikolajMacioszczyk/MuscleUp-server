using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IWorkerEmploymentRepository
    {
        Task<WorkerEmployment> GetWorkerEmploymentById(string workerEmploymentId, bool asTracking);

        Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId, bool includeInactive, bool asTracking);

        Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId, bool asTracking);

        Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment);

        Task<Result<WorkerEmployment>> TerminateWorkerEmployment(string workerEmploymentId);

        Task SaveChangesAsync();
    }
}
