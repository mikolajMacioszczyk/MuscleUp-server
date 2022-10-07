using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IAssignedPermissionService _assignedPermissionService;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public PermissionController(IMapper mapper,
            IAssignedPermissionService assignedPermissionService,
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _assignedPermissionService = assignedPermissionService;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("gympass-permissions/{gympassTypeId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<PermissionBaseDto>>> GetAllGympassPermissions([FromRoute] string gympassTypeId)
        {
            var permissionsResult = await _assignedPermissionService.GetAllGympassPermissions(gympassTypeId);

            if (permissionsResult.IsSuccess)
            {
                return Ok(_mapper.Map<IEnumerable<PermissionBaseDto>>(permissionsResult.Value));
            }
            else if (permissionsResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound($"Gympass Type with id {gympassTypeId} does not exists");
            }

            return BadRequest(permissionsResult.ErrorCombined);
        }

        [HttpPost("grant")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<AssignedPermissionDto>> GrantPermission([FromBody] GrantRevokePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var grantRequest = _mapper.Map<AssignedPermission>(model);

            var grantResult = await _assignedPermissionService.GrantPermission(grantRequest, fitnessClub.FitnessClubId);
            if (grantResult.IsSuccess)
            {
                return Ok(_mapper.Map<AssignedPermissionDto>(grantResult.Value));
            }
            else if (grantResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound("Gympass Type or Permission does not exists");
            }

            return BadRequest(grantResult.ErrorCombined);
        }

        [HttpDelete("revoke")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> RevokePermission([FromBody] GrantRevokePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClub.FitnessClubId;

            var revokeResult = await _assignedPermissionService.RevokePermission(model.PermissionId, fitnessClubId, model.GympassTypeId);

            if (revokeResult.IsSuccess)
            {
                return Ok();
            }
            else if (revokeResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound("Gympass Type or Permission does not exists");
            }

            return BadRequest(revokeResult.ErrorCombined);
        }

        [HttpDelete("revoke-all/{permissionId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> RemovePermissionWithAllAssigements([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClub.FitnessClubId;

            var revokeResult = await _assignedPermissionService.RemovePermissionWithAllAssigements(permissionId, fitnessClubId);

            if (revokeResult.IsSuccess)
            {
                return Ok();
            }
            else if (revokeResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound("Gympass Type or Permission does not exists");
            }

            return BadRequest(revokeResult.ErrorCombined);
        }
    }
}
