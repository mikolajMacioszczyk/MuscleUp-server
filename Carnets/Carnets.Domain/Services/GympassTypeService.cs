using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class GympassTypeService : IGympassTypeService
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPermissionService<PerkPermission> _perkPermissionService;
        private readonly IPermissionService<ClassPermission> _classPermissionService;
        private readonly IPermissionRepository<ClassPermission> _classPermissionRepository;
        private readonly IPermissionRepository<PerkPermission> _perkPermissionRepository;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public GympassTypeService(IGympassTypeRepository gympassTypeRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            IPermissionRepository<ClassPermission> classPermissionRepository,
            IPermissionRepository<PerkPermission> perkPermissionRepository,
            IPaymentService paymentService,
            IPermissionService<PerkPermission> perkPermissionService,
            IPermissionService<ClassPermission> classPermissionService,
            IMapper mapper)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
            _classPermissionRepository = classPermissionRepository;
            _perkPermissionRepository = perkPermissionRepository;
            _paymentService = paymentService;
            _perkPermissionService = perkPermissionService;
            _classPermissionService = classPermissionService;
            _mapper = mapper;
        }

        public Task<IEnumerable<GympassType>> GetAllGympassTypes(string fitnessClubId, bool onlyActive, int pageNumber, int pageSize) =>
            _gympassTypeRepository.GetAllGympassTypes(fitnessClubId, onlyActive, pageNumber, pageSize, false);

        public async Task<IEnumerable<GympassTypeWithPermissions>> GetAllGympassTypesWithPermissions(string fitnessClubId, bool onlyActive, int pageNumber, int pageSize)
        {
            var all = await _gympassTypeRepository.GetAllGympassTypes(fitnessClubId, onlyActive, pageNumber, pageSize, false);

            var allWithPermissions = new List<GympassTypeWithPermissions>();

            foreach (var gympassType in all)
            {
                allWithPermissions.Add(await AssignPermissionsToGympassType(gympassType));
            }

            return allWithPermissions;
        }

        public Task<GympassType> GetGympassTypeById(string gympassId) =>
            _gympassTypeRepository.GetGympassTypeById(gympassId, false);

        public async Task<GympassTypeWithPermissions> GetGympassTypeWithPermissionsById(string gympassId)
        {
            var gympassType = await _gympassTypeRepository.GetGympassTypeById(gympassId, false);
            if (gympassType is null)
            {
                return null;
            }

            return await AssignPermissionsToGympassType(gympassType);
        }

        public async Task<Result<GympassTypeWithPermissions>> CreateGympassType(
            GympassType gympassType, 
            IEnumerable<string> classPermissionsNames, 
            IEnumerable<string> perkPermissionsNames)
        {
            var allPermissionResult = await GetPermissionsByNames(classPermissionsNames, perkPermissionsNames);
            if (!allPermissionResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(allPermissionResult.Errors);
            }
            var (classPermissions, perkPermissions) = allPermissionResult.Value;

            var createResult = await _gympassTypeRepository.CreateGympassType(gympassType);

            if (!createResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(createResult.ErrorCombined);
            }

            var assignResult = await AssignAllGympassPermissions(classPermissions, perkPermissions, createResult.Value);
            if (!assignResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(assignResult.ErrorCombined);
            }

            await _paymentService.CreateProduct(createResult.Value);

            await _gympassTypeRepository.SaveChangesAsync();
            await _assignedPermissionRepository.SaveChangesAsync();

            var gympassWithPermissions = MapToGympassTypeWithPermissions(createResult.Value, classPermissionsNames, perkPermissionsNames);
            return new Result<GympassTypeWithPermissions>(gympassWithPermissions);
        }

        public async Task<Result<GympassTypeWithPermissions>> UpdateGympassType(GympassType gympassType)
        {
            var updateResult = await _gympassTypeRepository.UpdateGympassType(gympassType);

            if (updateResult.IsSuccess)
            {
                var linkedPermissionsResult = await _assignedPermissionRepository
                    .GetAllGympassPermissions(gympassType.GympassTypeId, true);

                if (!linkedPermissionsResult.IsSuccess)
                {
                    return new Result<GympassTypeWithPermissions>(linkedPermissionsResult.Errors);
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
                        return new Result<GympassTypeWithPermissions>(createResult.Errors);
                    }
                }

                await _paymentService.CreateProduct(updateResult.Value);

                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();

                var gympassWithPermissions = await AssignPermissionsToGympassType(updateResult.Value);
                return new Result<GympassTypeWithPermissions>(gympassWithPermissions);
            }

            return new Result<GympassTypeWithPermissions>(updateResult.Errors);
        }

        public async Task<Result<GympassTypeWithPermissions>> UpdateGympassTypeWithPermissions(
            GympassType gympassType, 
            IEnumerable<string> classPermissionNames, 
            IEnumerable<string> perkPermissionNames)
        {
            var allPermissionResult = await GetPermissionsByNames(classPermissionNames, perkPermissionNames);
            if (!allPermissionResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(allPermissionResult.Errors);
            }
            var (classPermissions, perkPermissions) = allPermissionResult.Value;

            var updateResult = await _gympassTypeRepository.UpdateGympassType(gympassType);

            if (!updateResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(updateResult.Errors);
            }

            var assignResult = await AssignAllGympassPermissions(classPermissions, perkPermissions, updateResult.Value);
            if (!assignResult.IsSuccess)
            {
                return new Result<GympassTypeWithPermissions>(assignResult.Errors);
            }

            await _paymentService.CreateProduct(updateResult.Value);

            await _gympassTypeRepository.SaveChangesAsync();
            await _assignedPermissionRepository.SaveChangesAsync();

            var gympassWithPermissions = MapToGympassTypeWithPermissions(updateResult.Value, classPermissionNames, perkPermissionNames);
            return new Result<GympassTypeWithPermissions>(gympassWithPermissions);
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

                await _paymentService.DeleteProduct(gympassTypeId);

                await _assignedPermissionRepository.SaveChangesAsync();
                await _gympassTypeRepository.SaveChangesAsync();
            }

            return deleteResult;
        }

        private async Task<Result<GympassType>> AssignAllGympassPermissions(
            IEnumerable<ClassPermission> classPermissions,
            IEnumerable<PerkPermission> perkPermissions,
            GympassType createdGympassType)
        {
            var assignResult = await AssignGympassPermissions(classPermissions, createdGympassType);
            if (!assignResult.IsSuccess)
            {
                return assignResult;
            }

            assignResult = await AssignGympassPermissions(perkPermissions, createdGympassType);
            if (!assignResult.IsSuccess)
            {
                return assignResult;
            }

            return new Result<GympassType>(createdGympassType);
        }

        private async Task<Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>> GetPermissionsByNames(
            IEnumerable<string> classPermissionsNames,
            IEnumerable<string> perkPermissionsNames)
        {
            // get all classPermissions
            var classPermissions = await _classPermissionRepository
                .GetAllPermissionsByNames(classPermissionsNames ?? Array.Empty<string>(), true);

            if (!classPermissions.IsSuccess)
            {
                return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>(classPermissions.Errors);
            }

            // get all perkPemrissions
            var perkPermissions = await _perkPermissionRepository
                .GetAllPermissionsByNames(perkPermissionsNames ?? Array.Empty<string>(), true);

            if (!perkPermissions.IsSuccess)
            {
                return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>(perkPermissions.Errors);
            }

            return new Result<(IEnumerable<ClassPermission>, IEnumerable<PerkPermission>)>((classPermissions.Value, perkPermissions.Value));
        }

        private async Task<Result<GympassType>> AssignGympassPermissions<TPermission>(
            IEnumerable<TPermission> permissions, GympassType gympassType)
            where TPermission : PermissionBase
        {
            foreach (var permission in permissions)
            {
                var assigement = new AssignedPermission()
                {
                    GympassTypeId = gympassType.GympassTypeId,
                    GympassType = gympassType,
                    PermissionId = permission.PermissionId,
                    Permission = permission
                };

                var assignResult = await _assignedPermissionRepository.CreateAssignedPermission(assigement);

                if (!assignResult.IsSuccess)
                {
                    return new Result<GympassType>(assignResult.Errors);
                }
            }

            return new Result<GympassType>(gympassType);
        }

        private async Task<GympassTypeWithPermissions> AssignPermissionsToGympassType(GympassType gympassType)
        {
            var gympassWithPermissions = _mapper.Map<GympassTypeWithPermissions>(gympassType);

            // class
            gympassWithPermissions.ClassPermissions = (await _classPermissionService.GetAllGympassTypePermissions(gympassType.GympassTypeId))
                .Select(p => p.PermissionName);
            // perk
            gympassWithPermissions.PerkPermissions = (await _perkPermissionService.GetAllGympassTypePermissions(gympassType.GympassTypeId))
                .Select(p => p.PermissionName);

            return gympassWithPermissions;
        }

        private GympassTypeWithPermissions MapToGympassTypeWithPermissions(GympassType source,
            IEnumerable<string> classPermissionNames,
            IEnumerable<string> perkPermissionNames)
        {
            var gympassWithPermissions = _mapper.Map<GympassTypeWithPermissions>(source);
            gympassWithPermissions.ClassPermissions = classPermissionNames;
            gympassWithPermissions.PerkPermissions = perkPermissionNames;

            return gympassWithPermissions;
        }
    }
}
