using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IWorkerEmploymentRepository
    {
        Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments();

        Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment);
    }
}
