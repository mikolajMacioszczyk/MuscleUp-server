using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Interfaces
{
    public interface IFitnessClubService
    {
        Task<IEnumerable<FitnessClub>> GetAll();
        Task<FitnessClub> GetById(string fitnessClubId);
        Task<Result<FitnessClub>> Create(FitnessClub fitnessClub);
        Task<Result<bool>> Delete(string fitnessClubId);
    }
}
