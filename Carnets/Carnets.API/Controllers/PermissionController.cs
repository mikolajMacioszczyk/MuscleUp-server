using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // TODO: Validate with worker's fitness club
    public class PermissionController : ControllerBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IFitnessClubHttpService _fitnessClubHttpService;
        private readonly IMapper _mapper;

        public PermissionController(IMapper mapper, 
            IAssignedPermissionRepository assignedPermissionRepository, 
            IFitnessClubHttpService fitnessClubHttpService)
        {
            _mapper = mapper;
            _assignedPermissionRepository = assignedPermissionRepository;
            _fitnessClubHttpService = fitnessClubHttpService;
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

        [HttpPost("grant/{workerId}")]
        public async Task<ActionResult<AssignedPermissionDto>> GrantPermission([FromRoute] string workerId, [FromBody] GrantRevokePermissionDto model)
        {
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

        [HttpDelete("revoke/{workerId}")]
        public async Task<ActionResult> RevokePermission([FromRoute] string workerId, [FromBody] GrantRevokePermissionDto model)
        {
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

        [HttpDelete("revokeAll/{permissionId}/{workerId}")]
        public async Task<ActionResult> RemovePermissionWithAllAssigements([FromRoute] string permissionId, [FromRoute] string workerId)
        {
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
