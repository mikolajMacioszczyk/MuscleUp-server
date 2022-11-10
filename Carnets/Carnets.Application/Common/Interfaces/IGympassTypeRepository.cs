using Carnets.Domain.Models;
using Common.Models;
using System.Linq.Expressions;

namespace Carnets.Application.Interfaces
{
    public interface IGympassTypeRepository
    {
        Task<GympassType> GetGympassTypeById(string gympassId, bool asTracking);

        Task<IEnumerable<GympassType>> GetAllGympassTypes<T>(
            string fitnessClubId, 
            bool onlyActive,
            int pageNumber, 
            int pageSize, 
            bool asTracking,
            Expression<Func<GympassType, T>> orderBy);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId);

        Task SaveChangesAsync();
    }
}
