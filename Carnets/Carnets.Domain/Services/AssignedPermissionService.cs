using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class AssignedPermissionService : IAssignedPermissionService
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IPermissionRepository<AllowedEntriesPermission> _allowedEntriesPermissionRepository;
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;
        private readonly IPermissionRepository<TimePermissionEntry> _timeEntryPermissionRepository;

        public AssignedPermissionService(
            IAssignedPermissionRepository assignedPermissionRepository,
            IGympassTypeRepository gympassTypeRepository, 
            IPermissionRepository<AllowedEntriesPermission> allowedEntriesPermissionRepository, 
            IPermissionRepository<ClassPermission> classPermissionRepository, 
            IPermissionRepository<TimePermissionEntry> timeEntryPermissionRepository)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
            _gympassTypeRepository = gympassTypeRepository;
            _allowedEntriesPermissionRepository = allowedEntriesPermissionRepository;
            _classPermissionRepository = classPermissionRepository;
            _timeEntryPermissionRepository = timeEntryPermissionRepository;
        }

        public Task<IEnumerable<AssignedPermission>> GetAllByPermission(string permissionId) =>
            _assignedPermissionRepository.GetAllByPermission(permissionId, false);

        public async Task<Result<IEnumerable<PermissionBase>>> GetAllGympassPermissions(string gympassTypeId)
        {
            var gympass = await _gympassTypeRepository.GetGympassTypeById(gympassTypeId, false);
            if (gympass is null)
            {
                return new Result<IEnumerable<PermissionBase>>(Common.CommonConsts.NOT_FOUND);
            }

            var result = await _assignedPermissionRepository.GetAllGympassPermissions(gympassTypeId, false);

            if (result.IsSuccess)
            {
                return new Result<IEnumerable<PermissionBase>>(result.Value.Select(a => a.Permission));
            }

            return new Result<IEnumerable<PermissionBase>>(result.ErrorCombined);
        }
            

        // Permission may be granted also to inactive gympass types
        public async Task<Result<AssignedPermission>> GrantPermission(AssignedPermission grantRequest, string fitnessClubId)
        {
            var gympassTypeFromDb = await _gympassTypeRepository.GetGympassTypeById(grantRequest.GympassTypeId, true);
            if (gympassTypeFromDb?.FitnessClubId != fitnessClubId)
            {
                gympassTypeFromDb = null;
            }

            var permission = await GetPermissionById(grantRequest.PermissionId, fitnessClubId, true);

            if (permission is null || gympassTypeFromDb is null)
            {
                return new Result<AssignedPermission>(Common.CommonConsts.NOT_FOUND);
            }

            var createResult = await _assignedPermissionRepository.CreateAssignedPermission(grantRequest);

            if (createResult.IsSuccess)
            {
                await _assignedPermissionRepository.SaveChangesAsync();
            }

            return createResult;
        }

        public async Task<Result<bool>> RevokePermission(string permissionId, string fitnessClubId, string gympassTypeId)
        {
            var removeResult = await _assignedPermissionRepository.RemovePermission(permissionId, gympassTypeId, fitnessClubId);

            if (removeResult.IsSuccess)
            {
                await _assignedPermissionRepository.SaveChangesAsync();
            }

            return removeResult;
        }

        public async Task<Result<bool>> RemovePermissionWithAllAssigements(string permissionId, string fitnessClubId)
        {
            var permissionFromDb = await GetPermissionById(permissionId, fitnessClubId, true);

            if (permissionFromDb is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            var assignedPermissions = await _assignedPermissionRepository
                .GetAllByPermission(permissionId, true);

            foreach (var assignedPermission in assignedPermissions)
            {
                await _assignedPermissionRepository
                    .RemovePermission(permissionId, assignedPermission.GympassTypeId, fitnessClubId);
            }

            switch (permissionFromDb.PermissionType)
            {
                case Enums.PermissionType.TimePermissionEntry:
                    await _timeEntryPermissionRepository.DeletePermission(permissionId, fitnessClubId);
                    await _timeEntryPermissionRepository.SaveChangesAsync();
                    break;
                case Enums.PermissionType.AllowedEntriesPermission:
                    await _allowedEntriesPermissionRepository.DeletePermission(permissionId, fitnessClubId);
                    await _allowedEntriesPermissionRepository.SaveChangesAsync();
                    break;
                case Enums.PermissionType.ClassPermission:
                    await _classPermissionRepository.DeletePermission(permissionId, fitnessClubId);
                    await _classPermissionRepository.SaveChangesAsync();
                    break;
            }

            // TODO: Does exists pattern to commit transactions?
            await _assignedPermissionRepository.SaveChangesAsync();

            return new Result<bool>(true);
        }

        private async Task<PermissionBase> GetPermissionById(string permissionId, string fitnessClubId, bool asTracking)
        {
            var allowedEntriesPermissionFromDb = await _allowedEntriesPermissionRepository
                .GetPermissionById(permissionId, asTracking);

            var classPermissionFromDb = await _classPermissionRepository.GetPermissionById(permissionId, asTracking);

            var timePermissionFromDb = await _timeEntryPermissionRepository.GetPermissionById(permissionId, asTracking);

            PermissionBase resultPermission = null;

            if (allowedEntriesPermissionFromDb != null)
            {
                resultPermission = allowedEntriesPermissionFromDb;
            }
            else if (classPermissionFromDb != null)
            {
                resultPermission = classPermissionFromDb;
            }
            else if (timePermissionFromDb != null)
            {
                resultPermission = timePermissionFromDb;
            }

            if (resultPermission?.FitnessClubId != fitnessClubId)
            {
                resultPermission = null;
            }

            return resultPermission;
        }
    }
}
