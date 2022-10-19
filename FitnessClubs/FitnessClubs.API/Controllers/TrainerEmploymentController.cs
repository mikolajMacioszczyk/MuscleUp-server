using AutoMapper;
using Common.BaseClasses;
using Common.Enums;
using FitnessClubs.Application.TrainerEmployments.Commands;
using FitnessClubs.Application.TrainerEmployments.Dtos;
using FitnessClubs.Application.TrainerEmployments.Queries;
using FitnessClubs.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    public class TrainerEmploymentController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public TrainerEmploymentController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{fitnessClubId}")]
        [Authorize(Roles = nameof(RoleType.Trainer) + "," + nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<TrainerEmploymentDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllTrainerEmploymentsQuery()
            {
                FitnessClubId = fitnessClubId,
                IncludeInactive = includeInactive
            };

            var workerEmployment = await Mediator.Send(query);
            return Ok(_mapper.Map<IEnumerable<TrainerEmploymentDto>>(workerEmployment));
        }

        [HttpPost()]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<TrainerEmploymentDto>> CreateTrainerEmployment(CreateTrainerEmploymentDto model)
        {
            var command = new CreateTrainerEmploymentCommand()
            {
                TrainerEmployment = _mapper.Map<TrainerEmployment>(model)
            };

            var createResult = await Mediator.Send(command);

            if (createResult.IsSuccess)
            {
                return Ok(_mapper.Map<TrainerEmploymentDto>(createResult.Value));
            }
            else if (createResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(createResult.ErrorCombined);
        }

        [HttpPut("{trainerEmploymentId}")]
        [Authorize(Roles = nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator))]
        public async Task<ActionResult<TrainerEmploymentDto>> TerminateTrainerEmployment([FromRoute] string trainerEmploymentId)
        {
            var command = new TerminateTrainerEmploymentCommand()
            {
                EmploymentId = trainerEmploymentId
            };

            var terminateResult = await Mediator.Send(command);

            if (terminateResult.IsSuccess)
            {
                return Ok(_mapper.Map<TrainerEmploymentDto>(terminateResult.Value));
            }
            else if (terminateResult.Errors?.Any(e => e.Equals(Common.CommonConsts.NOT_FOUND)) ?? false)
            {
                return NotFound();
            }

            return BadRequest(terminateResult.ErrorCombined);
        }
    }
}
