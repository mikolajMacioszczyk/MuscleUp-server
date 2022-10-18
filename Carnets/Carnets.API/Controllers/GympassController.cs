using AutoMapper;
using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Gympasses.Dtos;
using Carnets.Application.Gympasses.Queries;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = nameof(RoleType.Administrator) + "," + nameof(RoleType.Member))]
        public async Task<ActionResult<IEnumerable<GympassDto>>> GetAll()
        {
            var gympasses = await Mediator.Send(new GetAllGympassesQuery());

            return Ok(_mapper.Map<IEnumerable<GympassDto>>(gympasses));
        }

        [HttpGet("from-fitness-club")]
        [Authorize(Roles = nameof(RoleType.Worker))]
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
        [Authorize(Roles = AuthHelper.RoleAll)]
        public async Task<ActionResult<GympassDto>> GetById([FromRoute] string gympassId)
        {
            var gympass = await Mediator.Send(new GetGympassByIdQuery()
            {
                GympassId = gympassId
            });

            return Ok(_mapper.Map<GympassDto>(gympass));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Member))]
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
        [Authorize(Roles = nameof(RoleType.Member))]
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
        [Authorize(Roles = nameof(RoleType.Worker))]
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
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassDto>> ActivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var result = await Mediator.Send(new ActivateGympassCommand()
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

        [HttpPut("deactivate/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker))]
        public async Task<ActionResult<GympassDto>> DeactivateGympass([FromRoute] string gympassId)
        {
            var workerId = _httpAuthContext.UserId;
            await Mediator.Send(new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = workerId });

            var result = await Mediator.Send(new DeactivateGympassCommand()
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

        [HttpPut("entry/{gympassId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Member))]
        public async Task<ActionResult<GympassDto>> ReduceGympassEntries([FromRoute] string gympassId)
        {
            var result = await Mediator.Send(new ReduceGympassEntriesCommand()
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
    }
}
