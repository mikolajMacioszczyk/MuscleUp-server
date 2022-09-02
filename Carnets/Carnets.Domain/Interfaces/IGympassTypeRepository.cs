using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassTypeRepository
    {
        Task<GympassType> GetGympassById(string gympassId);

        Task<IEnumerable<GympassType>> GetAllActiveGympassTypes();

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId);
    }
}
