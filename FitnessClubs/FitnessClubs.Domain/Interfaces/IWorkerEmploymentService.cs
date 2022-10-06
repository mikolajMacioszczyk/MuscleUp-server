using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IWorkerEmploymentService
    {
        Task<WorkerEmployment> GetWorkerEmploymentById(string fitnessClubId, string workerId);

        Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId);

        Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId);

        Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment);
    }
}
