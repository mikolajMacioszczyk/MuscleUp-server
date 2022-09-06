using AutoMapper;
using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IMapper _mapper;

        public PermissionController(IMapper mapper, IAssignedPermissionRepository assignedPermissionRepository)
        {
            _mapper = mapper;
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        [HttpGet("gympassPermissions/{gympassId}")]
        public async Task<ActionResult<IEnumerable<AssignedPermissionDto>>> GetAllGympassPermissions(string gympassId)
        {
            var permissionsResult = await _assignedPermissionRepository.GetAllGympassPermissions(gympassId);

            if (permissionsResult.IsSuccess)
            {
                return Ok(_mapper.Map<IEnumerable<AssignedPermissionDto>>(permissionsResult.Value));
            }
            else if (permissionsResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound($"Gympass Type with id {gympassId} does not exists");
            }

            return BadRequest(permissionsResult.ErrorCombined);
        }

        [HttpPost("grant")]
        public async Task<ActionResult<AssignedPermissionDto>> GrantPermission([FromBody] GrantRevokePermissionDto model)
        {
            var grantRequest = _mapper.Map<AssignedPermission>(model);

            var grantResult = await _assignedPermissionRepository.GrantPermission(grantRequest);
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
        public async Task<ActionResult> RevokePermission([FromBody] GrantRevokePermissionDto model)
        {
            var revokeResult = await _assignedPermissionRepository.RevokePermission(model.PermissionId, model.GympassTypeId);

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
        public async Task<ActionResult> RemovePermissionWithAllAssigements([FromRoute] string permissionId)
        {
            var revokeResult = await _assignedPermissionRepository.RemovePermissionWithAllAssigements(permissionId);

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
