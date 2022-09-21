using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto, TCreatePermissionDto> : ControllerBase
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        protected SpecificPermissionControllerBase(
            IPermissionRepository<TPermission> permissionRepository,
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext httpAuthContext)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClubResult.Value.FitnessClubId;

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionRepository.GetAll(fitnessClubId)));
        }

        [HttpGet("{permissionId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> GetPermission([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var permission = await _permissionRepository.GetPermissionById(permissionId, fitnessClubResult.Value.FitnessClubId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> CreatePermission([FromBody] TCreatePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var permission = _mapper.Map<TPermission>(model);
            permission.FitnessClubId = fitnessClubResult.Value.FitnessClubId;

            var createResult = await _permissionRepository.CreatePermission(permission);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TPermissionDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{permissionId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> DeletePermission([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var deleteResult = await _permissionRepository.DeletePermission(permissionId, fitnessClubResult.Value.FitnessClubId);

            if (deleteResult.IsSuccess)
            {
                return Ok();
            }
            else if (deleteResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(deleteResult.ErrorCombined);
        }
    }
}
