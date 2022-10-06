using Common.Models;
using Common.Models.Dtos;

namespace Carnets.Domain.Interfaces
{
    public interface IFitnessClubHttpService
    {
        Task<Result<FitnessClubDto>> GetFitnessClubById(string fitnessClubId);

        Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId);

        Task<FitnessClubDto> EnsureWorkerCanManageFitnessClub(string workerId);
     
        Task<FitnessClubDto> EnsureFitnessClubExists(string fitnessClubId);
    }
}
