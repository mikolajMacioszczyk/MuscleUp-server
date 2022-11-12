using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Common.Models.Dtos;
using FitnessClubs.Application.TrainerEmployments.Commands;
using FitnessClubs.Application.TrainerEmployments.Dtos;
using FitnessClubs.Application.TrainerEmployments.Queries;
using FitnessClubs.Application.UserInvitations.Commands;
using FitnessClubs.Application.UserInvitations.Dtos;
using FitnessClubs.Application.WorkoutEmployments.Dtos;
using FitnessClubs.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubs.API.Controllers
{
    public class TrainerEmploymentController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HttpAuthContext _httpAuthContext;

        public TrainerEmploymentController(
            HttpAuthContext httpAuthContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
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

        [HttpPost("trainer-invitation")]
        [AuthorizeRoles(RoleType.Worker, RoleType.Owner, RoleType.Administrator)]
        public async Task<ActionResult<UserInvitationDto>> GenerateTrainerInvitation(GenerateUserInvitationDto model)
        {
            var invitationResult = await Mediator.Send(
                new GenerateUserInvitationCommand(model.FitnessClubId, model.Email, model.BaseInvitationLink, RoleType.Trainer));

            if (invitationResult.IsSuccess)
            {
                return Ok(_mapper.Map<UserInvitationDto>(invitationResult.Value));
            }

            return BadRequest(invitationResult.ErrorCombined);
        }

        [HttpPut("trainer-invitation")]
        [AuthorizeRoles(RoleType.Trainer)]
        public async Task<ActionResult<TrainerEmploymentDto>> ConsumeTrainerInvitation(ConsumeUserInvitationDto model)
        {
            var trainerId = _httpAuthContext.UserId;
            var employmentResult = await Mediator.Send(new ConsumeTrainerInvitationCommand(model.InvitationId, trainerId));

            if (employmentResult.IsSuccess)
            {
                return Ok(_mapper.Map<TrainerEmploymentDto>(employmentResult.Value));
            }

            return BadRequest(employmentResult.ErrorCombined);
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
