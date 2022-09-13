using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassTypeRepository
    {
        Task<GympassType> GetGympassById(string gympassId, string fitnessClubId);

        Task<IEnumerable<GympassType>> GetAllActiveGympassTypes(string fitnessClubId);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType, string fitnessClubId);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId, string fitnessClubId);
    }
}
