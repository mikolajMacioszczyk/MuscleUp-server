using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.Interfaces
{
    public interface IFitnessClubRepository
    {
        Task<IEnumerable<FitnessClub>> GetAll(bool asTracking);
        Task<FitnessClub> GetById(string fitnessClubId, bool asTracking);
        Task<Result<FitnessClub>> Create(FitnessClub fitnessClub);
        Task<Result<bool>> Delete(string fitnessClubId);
        Task SaveChangesAsync();
    }
}
