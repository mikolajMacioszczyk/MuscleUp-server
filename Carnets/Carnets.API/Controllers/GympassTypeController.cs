using AutoMapper;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.GympassTypes.Commands;
using Carnets.Application.GympassTypes.Dtos;
using Carnets.Application.GympassTypes.Queries;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class GympassTypeController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public GympassTypeController(IMapper mapper, HttpAuthContext authContext)
        {
            _mapper = mapper;
            _httpAuthContext = authContext;
        }

        [HttpGet("{gympassTypeId}")]
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<GympassTypeDto>> GetGympassTypeById([FromRoute] string gympassTypeId)
        {
            var query = new GetGympassTypeWithPermissionsByIdQuery()
            {
                GympassTypeId = gympassTypeId
            };

            var gympassType = await Mediator.Send(query);
            if (gympassType != null)
            {
                return Ok(gympassType);
            }
            return NotFound();
        }

        [HttpGet("active-as-worker")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypesAsWorker(
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = CommonConsts.DefaultPageSize)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var query = new GetAllGympassTypesWithPermissionsQuery()
            {
                FitnessClubId = fitnessClub.FitnessClubId,
                OnlyActive = true,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var gympassTypes = await Mediator.Send(query);
            
            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpGet("active/{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Member) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<GympassTypeDto>>> GetActiveGympassTypes([FromRoute] string fitnessClubId,
            [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = CommonConsts.DefaultPageSize)
        {
            await Mediator.Send(new EnsureFitnessClubExistsQuery()
            {
                FitnessClubId = fitnessClubId
            });

            var query = new GetAllGympassTypesWithPermissionsQuery()
            {
                FitnessClubId = fitnessClubId,
                OnlyActive = true,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var gympassTypes = await Mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<GympassTypeDto>>(gympassTypes));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> CreateGympassType([FromBody] CreateGympassTypeDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });
            
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.FitnessClubId = fitnessClub.FitnessClubId;

            var command = new CreateGympassTypeCommand()
            {
                GympassType = gympassType,
                ClassPermissionsNames = model.ClassPermissions,
                PerkPermissionsNames = model.PerkPermissions
            };

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassType([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeDto model)
        {
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await Mediator.Send(new UpdateGympassTypeCommand()
            {
                GympassType = gympassType
            });

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(updateResult.Value));
            }
            else if (updateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(updateResult.ErrorCombined);
        }

        [HttpPut("with-permissions/{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassTypeDto>> UpdateGympassTypeWithPermissions([FromRoute] string gympassTypeId, [FromBody] UpdateGympassTypeWithPermissionsDto model)
        {
            var gympassType = _mapper.Map<GympassType>(model);
            gympassType.GympassTypeId = gympassTypeId;

            var updateResult = await Mediator.Send(new UpdateGympassTypeWithPermissionsCommand()
            {
                GympassType = gympassType,
                ClassPermissions = model.ClassPermissions,
                PerkPermissions = model.PerkPermissions
            });

            if (updateResult.IsSuccess)
            {
                return Ok(_mapper.Map<GympassTypeDto>(updateResult.Value));
            }
            else if (updateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(updateResult.ErrorCombined);
        }

        [HttpDelete("{gympassTypeId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult> DeleteGympassType([FromRoute] string gympassTypeId)
        {
            var command = new DeleteGympassTypeCommand()
            {
                GympassTypeId = gympassTypeId
            };

            var deleteResult = await Mediator.Send(command);

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
