using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassTypeService
    {
        Task<GympassType> GetGympassTypeById(string gympassId);

        Task<GympassTypeWithPermissions> GetGympassTypeWithPermissionsById(string gympassId);

        Task<IEnumerable<GympassType>> GetAllGympassTypes(string fitnessClubId, bool onlyActive, int pageNumber, int pageSize);
        
        Task<IEnumerable<GympassTypeWithPermissions>> GetAllGympassTypesWithPermissions(string fitnessClubId, bool onlyActive, int pageNumber, int pageSize);

        Task<Result<GympassType>> CreateGympassType(GympassType gympassType, IEnumerable<string> classPermissions, IEnumerable<string> perkPermissions);

        Task<Result<GympassType>> UpdateGympassType(GympassType gympassType);

        Task<Result<GympassType>> UpdateGympassTypeWithPermissions(GympassType gympassType, IEnumerable<string> classPermissions, IEnumerable<string> perkPermissions);

        Task<Result<bool>> DeleteGympassType(string gympassTypeId);
    }
}
