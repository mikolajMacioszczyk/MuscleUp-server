using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassTypeRepository
    {
        Task<GympassType> GetGympassById(string gympassId);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);
    }
}
