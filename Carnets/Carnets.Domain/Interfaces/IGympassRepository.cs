using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassRepository
    {
        Task<IEnumerable<Gympass>> GetAll(bool asTracking);

        Task<IEnumerable<Gympass>> GetAllFromFitnessClub(string fitnessClubId, bool asTracking);

        Task<IEnumerable<Gympass>> GetAllForMember(string memberId, bool asTracking);

        Task<Gympass> GetById(string gympassId, bool asTracking);

        Task<Result<Gympass>> CreateGympass(string gympassTypeId, Gympass created);

        Task<Result<Gympass>> UpdateGympass(Gympass updated);

        Task SaveChangesAsync();
    }
}
