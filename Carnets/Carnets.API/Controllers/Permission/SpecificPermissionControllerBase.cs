using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto, TCreatePermissionDto> : ControllerBase
        where TPermission : PermissionBase
    {
        private readonly IPermissionService<TPermission> _permissionService;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        protected SpecificPermissionControllerBase(
            IPermissionService<TPermission> permissionService,
            IMapper mapper,
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext httpAuthContext)
        {
            _permissionService = permissionService;
            _mapper = mapper;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClub.FitnessClubId;

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionService.GetAll(fitnessClubId)));
        }

        [HttpGet("all-as-admin/{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermisions([FromRoute] string fitnessClubId)
        {
            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(await _permissionService.GetAll(fitnessClubId)));
        }

        [HttpGet("{permissionId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult> GetPermissionById([FromRoute] string permissionId)
        {
            var permission = await _permissionService.GetPermissionById(permissionId);
            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpGet("by-gympass-type/{gympassTypeId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllGympassTypePermisions([FromRoute] string gympassTypeId)
        {
            var allPermissions = await _permissionService.GetAllGympassTypePermissions(gympassTypeId);
            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(allPermissions));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> CreatePermission([FromBody] TCreatePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var permission = _mapper.Map<TPermission>(model);
            permission.FitnessClubId = fitnessClub.FitnessClubId;

            var createResult = await _permissionService.CreatePermission(permission);

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
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var deleteResult = await _permissionService.DeletePermission(permissionId, fitnessClub.FitnessClubId);

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
