using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IWorkerEmploymentRepository
    {
        Task<WorkerEmployment> GetWorkerEmploymentById(string fitnessClubId, string workerId, bool asTracking);

        Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId, bool asTracking);

        Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId, bool asTracking);

        Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment);

        Task SaveChangesAsync();
    }
}
