using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models.Dtos;
using FitnessClubs.Application.TrainerEmployments.Commands;
using FitnessClubs.Application.TrainerEmployments.Dtos;
using FitnessClubs.Application.TrainerEmployments.Queries;
using FitnessClubs.Domain.Models;
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
        [AuthorizeRoles(RoleType.Trainer, RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAllTrainersFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllEmployedTrainersQuery(fitnessClubId, includeInactive);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("details/{fitnessClubId}")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<TrainerEmploymentWithUserDataDto>>> GetAllEmploymentsFromFitnessClub([FromRoute] string fitnessClubId, [FromQuery] bool includeInactive = false)
        {
            var query = new GetAllTrainerEmploymentsQuery(fitnessClubId, includeInactive);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost()]
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
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
        [AuthorizeRoles(RoleType.Worker, RoleType.Administrator)]
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
