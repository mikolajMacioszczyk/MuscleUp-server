using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassTypeService
    {
        Task<GympassType> GetGympassTypeById(string gympassId);

        Task<IEnumerable<GympassType>> GetAllGympassTypes(string fitnessClubId, bool onlyActive);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId);
    }
}
