using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models.Dtos;
using FitnessClubs.Application.WorkoutEmployments.Commands;
using FitnessClubs.Application.WorkoutEmployments.Dtos;
using FitnessClubs.Application.WorkoutEmployments.Queries;
using FitnessClubs.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    public class WorkerEmploymentController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public WorkerEmploymentController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllWorkersFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllEmployedWorkersQuery(fitnessClubId, includeInactive);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("details/{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllWorkerEmploymentsQuery(fitnessClubId, includeInactive);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Owner, RoleType.Administrator)]
        public async Task<ActionResult<WorkerEmploymentDto>> CreateWorkerEmployment(CreateWorkerEmploymentDto model)
        {
            var command = new CreateWorkerEmploymentCommand()
            {
                WorkerEmployment = _mapper.Map<WorkerEmployment>(model)
            };

            var createResult = await Mediator.Send(command);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerEmploymentDto>(createResult.Value));
            }
            else if (createResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("{workerEmploymentId}")]
        [AuthorizeRoles(RoleType.Owner, RoleType.Administrator)]
        public async Task<ActionResult<WorkerEmploymentDto>> TerminateWorkerEmployment([FromRoute] string workerEmploymentId)
        {
            var command = new TerminateWorkerEmploymentCommand()
            {
                WorkerEmploymentId = workerEmploymentId
            };

            var terminateResult = await Mediator.Send(command);

            if (terminateResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerEmploymentDto>(terminateResult.Value));
            }
            else if (terminateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(terminateResult.ErrorCombined);
        }
    }
}
