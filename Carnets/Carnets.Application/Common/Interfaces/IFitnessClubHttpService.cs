using Common.Models;
using Common.Models.Dtos;

namespace Carnets.Application.Interfaces
{
    public interface IFitnessClubHttpService
    {
        Task<Result<FitnessClubDto>> GetFitnessClubById(string fitnessClubId);

        Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId);
    }
}
