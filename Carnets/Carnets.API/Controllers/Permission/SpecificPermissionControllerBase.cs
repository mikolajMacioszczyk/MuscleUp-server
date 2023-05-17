using AutoMapper;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.SpecificPermissions.Commands;
using Carnets.Application.SpecificPermissions.Queries;
using Carnets.Domain.Models;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public abstract class SpecificPermissionControllerBase<TPermission, TPermissionDto, TCreatePermissionDto> : ApiControllerBase
        where TPermission : PermissionBase
    {
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        protected SpecificPermissionControllerBase(IMapper mapper, HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermissions()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });
            var fitnessClubId = fitnessClub.FitnessClubId;

            var allPermissions = await Mediator.Send(new GetAllPermissionsQuery<TPermission>()
            {
                FitnessClubId = fitnessClubId
            });

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(allPermissions));
        }

        [HttpGet("all-from-fitness-club/{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Member)]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllPermissions([FromRoute] string fitnessClubId)
        {
            var allPermissions = await Mediator.Send(new GetAllPermissionsQuery<TPermission>()
            {
                FitnessClubId = fitnessClubId
            });

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(allPermissions));
        }

        [HttpGet("{permissionId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult> GetPermissionById([FromRoute] string permissionId)
        {
            var permission = await Mediator.Send(new GetPermissionById<TPermission>()
            {
                PermissionId = permissionId
            });

            if (permission != null)
            {
                return Ok(_mapper.Map<TPermissionDto>(permission));
            }
            return NotFound();
        }

        [HttpGet("by-gympass-type/{gympassTypeId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<TPermissionDto>>> GetAllGympassTypePermissions([FromRoute] string gympassTypeId)
        {
            var allPermissions = await Mediator.Send(new GetAllGympassTypePermissionsQuery<TPermission>()
            {
                GympassTypeId = gympassTypeId
            });

            return Ok(_mapper.Map<IEnumerable<TPermissionDto>>(allPermissions));
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult> CreatePermission([FromBody] TCreatePermissionDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var permission = _mapper.Map<TPermission>(model);
            permission.FitnessClubId = fitnessClub.FitnessClubId;

            var createResult = await Mediator.Send(new CreatePermissionCommand<TPermission>()
            {
                NewPermission = permission,
            });

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TPermissionDto>(createResult.Value));
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpDelete("{permissionId}")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult> DeletePermission([FromRoute] string permissionId)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var deleteResult = await Mediator.Send(new DeletePermissionCommand<TPermission>()
            {
                FitnessClubId = fitnessClub.FitnessClubId,
                PermissionId = permissionId
            });

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
