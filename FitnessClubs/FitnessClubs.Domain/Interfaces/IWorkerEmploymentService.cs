using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IWorkerEmploymentService
    {
        Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId, bool onlyActive);

        Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId);

        Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment);

        Task<Result<WorkerEmployment>> TerminateWorkerEmployment(string workerEmploymentId);
    }
}
