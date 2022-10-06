using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class GympassTypeService : IGympassTypeService
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public GympassTypeService(IGympassTypeRepository gympassTypeRepository, 
            IAssignedPermissionRepository assignedPermissionRepository)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public Task<IEnumerable<GympassType>> GetAllGympassTypes(string fitnessClubId, bool onlyActive) =>
            _gympassTypeRepository.GetAllGympassTypes(fitnessClubId, onlyActive, false);

        public Task<GympassType> GetGympassTypeById(string gympassId) =>
            _gympassTypeRepository.GetGympassTypeById(gympassId, false);

        public async Task<Result<GympassType>> CreateGympassType(GympassType gympassType)
        {
            var createResult = await _gympassTypeRepository.CreateGympassType(gympassType);

            if (createResult.IsSuccess)
            {
                await _gympassTypeRepository.SaveChangesAsync();
            }

            return createResult;
        }

        public async Task<Result<GympassType>> UpdateGympassType(GympassType gympassType)
        {
            var updateResult = await _gympassTypeRepository.UpdateGympassType(gympassType);

            if (updateResult.IsSuccess)
            {
                var linkedPermissionsResult = await _assignedPermissionRepository
                    .GetAllGympassPermissions(gympassType.GympassTypeId, true);

                if (!linkedPermissionsResult.IsSuccess)
                {
                    return new Result<GympassType>(linkedPermissionsResult.ErrorCombined);
                }

                foreach (var oldAssignedPermission in linkedPermissionsResult.Value)
                {
                    var newAssignedPermission = new AssignedPermission
                    {
                        Permission = oldAssignedPermission.Permission,
                        PermissionId = oldAssignedPermission.PermissionId,
                        GympassType = updateResult.Value,
                        GympassTypeId = updateResult.Value.GympassTypeId
                    };

                    var createResult = await _assignedPermissionRepository.CreateAssignedPermission(newAssignedPermission);
                    
                    if (!createResult.IsSuccess)
                    {
                        return new Result<GympassType>(createResult.ErrorCombined);
                    }
                }
                
                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();
            }

            return updateResult;
        }

        public async Task<Result<bool>> DeleteGympassType(string gympassTypeId)
        {
            var deleteResult = await _gympassTypeRepository.DeleteGympassType(gympassTypeId);

            if (deleteResult.IsSuccess)
            {
                var linkedPermissionsResult = await _assignedPermissionRepository
                    .GetAllGympassPermissions(gympassTypeId, true);

                if (!linkedPermissionsResult.IsSuccess)
                {
                    return new Result<bool>(linkedPermissionsResult.ErrorCombined);
                }

                foreach (var assignedPermission in linkedPermissionsResult.Value)
                {
                    var deleteAssignedResult = await _assignedPermissionRepository.RemovePermission(assignedPermission.PermissionId, 
                        assignedPermission.GympassTypeId, assignedPermission.Permission?.FitnessClubId);

                    if (!deleteAssignedResult.IsSuccess)
                    {
                        return deleteAssignedResult;
                    }
                }

                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();
            }

            return deleteResult;
        }
    }
}
