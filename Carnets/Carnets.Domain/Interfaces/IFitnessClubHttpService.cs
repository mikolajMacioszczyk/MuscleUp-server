using Common.Models;
using Common.Models.Dtos;

namespace Carnets.Domain.Interfaces
{
    public interface IFitnessClubHttpService
    {
        Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId);

        Task<Result<FitnessClubDto>> EnsureWorkerCanManageFitnessClub(string workerId);
    }
}
