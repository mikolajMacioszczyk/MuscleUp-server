using AutoMapper;
using Carnets.Application.AssignedPermissions.Commands;
using Carnets.Application.AssignedPermissions.Dtos;
using Carnets.Application.Dtos.Permission;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Permissions.Queries;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class PermissionController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public PermissionController(IMapper mapper, HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("gympass-permissions/{gympassTypeId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<PermissionBaseDto>>> GetAllGympassPermissions([FromRoute] string gympassTypeId)
        {
            var permissionsResult = await Mediator.Send(new GetAllGympassPermissionsQuery()
            {
                GympassTypeId = gympassTypeId
            });

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
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<AssignedPermissionDto>> GrantPermission([FromBody] GrantRevokePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var grantResult = await Mediator.Send(new GrantPermissionCommand()
            {
                Model = model,
                FitnessClubId = fitnessClub.FitnessClubId
            });

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
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult> RevokePermission([FromBody] GrantRevokePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });
            var fitnessClubId = fitnessClub.FitnessClubId;

            var revokeResult = await Mediator.Send(new RevokePermissionCommand()
            {
                PermissionId = model.PermissionId,
                FitnessClubId = fitnessClubId,
                GympassTypeId = model.GympassTypeId
            });

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
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult> RemovePermissionWithAllAssigements([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });
            var fitnessClubId = fitnessClub.FitnessClubId;

            var revokeResult = await Mediator.Send(new RemovePermissionWithAllAssigementsCommand()
            {
                FitnessClubId = fitnessClubId,
                PermissionId = permissionId
            });

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
