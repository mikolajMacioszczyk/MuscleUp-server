using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Common.Models.Dtos;
using FitnessClubs.Application.UserInvitations.Commands;
using FitnessClubs.Application.UserInvitations.Dtos;
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
        private readonly HttpAuthContext _httpAuthContext;

        public WorkerEmploymentController(IMapper mapper, HttpAuthContext httpAuthContext)
        {
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
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

        [HttpPost("worker-invitation")]
        [AuthorizeRoles(RoleType.Owner, RoleType.Administrator)]
        public async Task<ActionResult<UserInvitationDto>> GenerateWorkerInvitation(GenerateUserInvitationDto model)
        {
            var invitationResult = await Mediator.Send(
                new GenerateWorkerInvitationCommand(model.FitnessClubId, model.Email, model.BaseInvitationLink));

            if (invitationResult.IsSuccess)
            {
                return Ok(_mapper.Map<UserInvitationDto>(invitationResult.Value));
            }

            return BadRequest(invitationResult.ErrorCombined);
        }

        [HttpPut("worker-invitation")]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<WorkerEmploymentDto>> ConsumeWorkerInvitation(ConsumeUserInvitationDto model)
        {
            var workerId = _httpAuthContext.UserId;
            var employmentResult = await Mediator.Send(new ConsumeWorkerInvitationCommand(model.InvitationId, workerId));

            if (employmentResult.IsSuccess)
            {
                return Ok(_mapper.Map<WorkerEmploymentDto>(employmentResult.Value));
            }

            return BadRequest(employmentResult.ErrorCombined);
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
