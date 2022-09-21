using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public PermissionController(IMapper mapper,
            IAssignedPermissionRepository assignedPermissionRepository,
            IFitnessClubHttpService fitnessClubHttpService, 
            HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _assignedPermissionRepository = assignedPermissionRepository;
            _fitnessClubHttpService = fitnessClubHttpService;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("gympassPermissions/{gympassId}")]
        public async Task<ActionResult<IEnumerable<PermissionBaseDto>>> GetAllGympassPermissions([FromRoute] string gympassId)
        {
            var permissionsResult = await _assignedPermissionRepository.GetAllGympassPermissions(gympassId);

            if (permissionsResult.IsSuccess)
            {
                return Ok(_mapper.Map<IEnumerable<PermissionBaseDto>>(permissionsResult.Value));
            }
            else if (permissionsResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound($"Gympass Type with id {gympassId} does not exists");
            }

            return BadRequest(permissionsResult.ErrorCombined);
        }

        [HttpPost("grant")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<AssignedPermissionDto>> GrantPermission([FromBody] GrantRevokePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);

            var grantRequest = _mapper.Map<AssignedPermission>(model);

            var grantResult = await _assignedPermissionRepository.GrantPermission(grantRequest, fitnessClubResult.Value.FitnessClubId);
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
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClubResult.Value.FitnessClubId;

            var revokeResult = await _assignedPermissionRepository.RevokePermission(model.PermissionId, fitnessClubId, model.GympassTypeId);

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

        [HttpDelete("revokeAll/{permissionId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> RemovePermissionWithAllAssigements([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClubResult = await _fitnessClubHttpService.EnsureWorkerCanManageFitnessClub(workerId);
            var fitnessClubId = fitnessClubResult.Value.FitnessClubId;

            var revokeResult = await _assignedPermissionRepository.RemovePermissionWithAllAssigements(permissionId, fitnessClubId);

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
