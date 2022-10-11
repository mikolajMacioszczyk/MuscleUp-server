using AutoMapper;
using Common.BaseClasses;
using Common.Enums;
using FitnessClubs.Application.WorkoutEmployments.Commands;
using FitnessClubs.Application.WorkoutEmployments.Dtos;
using FitnessClubs.Application.WorkoutEmployments.Queries;
using FitnessClubs.Domain.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<WorkerEmploymentDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllWorkerEmploymentsQuery()
            {
                FitnessClubId = fitnessClubId,
                IncludeInactive = includeInactive
            };

            var workerEmployment = await Mediator.Send(query);
            return Ok(_mapper.Map<IEnumerable<WorkerEmploymentDto>>(workerEmployment));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
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
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
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
