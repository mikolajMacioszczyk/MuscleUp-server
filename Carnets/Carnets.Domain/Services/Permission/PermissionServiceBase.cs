using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Exceptions;
using Common.Models;

namespace Carnets.Domain.Services.Permission
{
    public class PermissionServiceBase<TPermission> : IPermissionService<TPermission>
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public PermissionServiceBase(IPermissionRepository<TPermission> permissionRepository, 
            IAssignedPermissionRepository assignedPermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public Task<IEnumerable<TPermission>> GetAll(string fitnessClubId) =>
            _permissionRepository.GetAll(fitnessClubId, false);


        public Task<TPermission> GetPermissionById(string permissionId) =>
            _permissionRepository.GetPermissionById(permissionId, false);

        public async Task<IEnumerable<TPermission>> GetAllGympassTypePermissions(string gympassTypeId)
        {
            var allResult = await _assignedPermissionRepository.GetAllGympassPermissions(gympassTypeId, false);

            if (allResult.IsSuccess)
            {
                var allIds = allResult.Value.Select(a => a.PermissionId).ToArray();

                return await _permissionRepository.GetPermissionByIds(allIds, false);
            }

            throw new BadRequestException(allResult.ErrorCombined);
        }

        public async Task<Result<TPermission>> CreatePermission(TPermission newPermission)
        {
            var result = await _permissionRepository.CreatePermission(newPermission);

            if (result.IsSuccess)
            {
                await _permissionRepository.SaveChangesAsync();
            }

            return result;
        }

        public async Task<Result<bool>> DeletePermission(string permissionId, string fitnessClubId)
        {
            var result = await _permissionRepository.DeletePermission(permissionId, fitnessClubId);

            if (result.IsSuccess)
            {
                var connectedPermissions = await _assignedPermissionRepository
                    .GetAllByPermission(permissionId, true);

                // TODO: if someone uses this permission, broadcast message

                foreach (var assignedPermission in connectedPermissions)
                {
                    var removeAssignedPermissionResult = await _assignedPermissionRepository
                        .RemovePermission(assignedPermission.PermissionId, assignedPermission.GympassTypeId, fitnessClubId);
                    
                    if (!removeAssignedPermissionResult.IsSuccess)
                    {
                        return removeAssignedPermissionResult;
                    }
                }

                await _assignedPermissionRepository.SaveChangesAsync();
                await _permissionRepository.SaveChangesAsync();
            }

            return result;
        }
    }
}
