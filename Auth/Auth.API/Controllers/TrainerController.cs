using Auth.Application.Trainer.Commands;
using Auth.Application.Trainer.Dtos;
using Auth.Application.Trainer.Queries;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models;
using Common.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    public class TrainerController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;

        public TrainerController(HttpAuthContext httpAuthContext)
        {
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("all")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAllTrainers()
        {
            return Ok(await Mediator.Send(new GetAllTrainersQuery()));
        }

        [HttpGet("by-ids/{userIds}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetAllTrainersWithIds(
            [FromRoute] string userIds,
            [FromQuery] string separator = ",")
        {
            return Ok(await Mediator.Send(new GetAllTrainersWithIdsQuery()
            {
                UserIds = userIds,
                Separator = separator
            }));
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Trainer)]
        public async Task<ActionResult<TrainerDto>> GetTrainerData()
        {
            var trainerId = _httpAuthContext.UserId;
            var trainer = await Mediator.Send(new GetTrainerByIdQuery(trainerId));

            return trainer is null ? NotFound() : Ok(trainer);
        }

        [HttpGet("find/{userId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<MemberDto>> GetTrainerById([FromRoute] string userId)
        {
            var member = await Mediator.Send(new GetTrainerByIdQuery(userId));

            return member is null ? NotFound() : Ok(member);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TrainerDto>> RegisterTrainer([FromBody] RegisterTrainerDto registerDto)
        {
            var command = new RegisterTrainerCommand() { RegisterDto = registerDto };

            return Ok(await Mediator.Send(command));
        }

        [HttpPut()]
        [AuthorizeRoles(RoleType.Trainer)]
        public async Task<ActionResult<TrainerDto>> UpdateTrainer([FromBody] UpdateTrainerDto updateDto)
        {
            var command = new UpdateTrainerCommand() { UpdateDto = updateDto };

            return Ok(await Mediator.Send(command));
        }
    }
}
