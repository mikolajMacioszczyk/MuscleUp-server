using AutoMapper;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Gympasses.Dtos;
using Carnets.Application.Gympasses.Queries;
using Carnets.Domain.Models;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carnets.API.Controllers
{
    public class GympassController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public GympassController(IMapper mapper, HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Member)]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAll()
        {
            IEnumerable<Gympass> gympasses;

            if (_httpAuthContext.UserRole == RoleType.Member)
            {
                var memberId = _httpAuthContext.UserId;
                gympasses = await Mediator.Send(new GetAllMemberGympassesQuery(memberId));
            }
            else
            {
                gympasses = await Mediator.Send(new GetAllGympassesQuery());
            }

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("by-member/{memberId}")]
        [AuthorizeRoles(RoleType.Administrator, RoleType.Worker)]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAllMembersGympasses([FromRoute] string memberId)
        {
            var gympasses = await Mediator.Send(new GetAllMemberGympassesQuery(memberId));

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("from-fitness-club")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAllFromFitnessClub()
        {
            var workerId = _httpAuthContext.UserId;
            var fitnessClub = await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery()
            {
                WorkerId = workerId,
            });

            var gympasses = await Mediator.Send(new GetAllFromFitnessClubQuery()
            {
                FitnessClubId = fitnessClub.FitnessClubId
            });

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("{gympassId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<GympassDto>> GetById([FromRoute] string gympassId)
        {
            var gympass = await Mediator.Send(new GetGympassByIdQuery()
            {
                GympassId = gympassId
            });

            return gympass is null ? NotFound() : Ok(_mapper.Map<GympassDto>(gympass));
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<GympassWithSessionDto>> Create([FromBody] CreateGympassDto model)
        {
            var memberId = _httpAuthContext.UserId;

            var createResult = await Mediator.Send(new CreateGympassCommand()
            {
                Model = model,
                UserId = memberId
            });

            if (createResult.IsSuccess)
            {
                return Ok(createResult.Value);
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("cancel/{gympassId}")]
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<GympassDto>> CancelGympass([FromRoute] string gympassId)
        {
            var result = await Mediator.Send(new CancelGympassCommand()
            {
                GympassId = gympassId
            });

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("cancel-as-worker/{gympassId}")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<GympassDto>> CancelGympassAsWorker([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId});

            var result = await Mediator.Send(new CancelGympassCommand()
            {
                GympassId = gympassId
            });

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("activate/{gympassId}")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<GympassDto>> ActivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var result = await Mediator.Send(new ActivateGympassCommand(gympassId));

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }

        [HttpPut("deactivate/{gympassId}")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<GympassDto>> DeactivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var result = await Mediator.Send(new DeactivateGympassCommand(gympassId));

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<GympassDto>(result.Value));
            }
            else if (result.Errors.Contains(Common.CommonConsts.NOT_FOUND))
            {
                return NotFound();
            }

            return BadRequest(result.ErrorCombined);
        }
    }
}
