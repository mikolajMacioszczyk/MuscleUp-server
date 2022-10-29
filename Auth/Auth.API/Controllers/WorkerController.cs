using Auth.Application.Workers.Commands;
using Auth.Application.Workers.Dtos;
using Auth.Application.Workers.Queries;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    public class WorkerController : ApiControllerBase
    {
        [HttpGet("all")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllWorkers()
        {
            return Ok(await Mediator.Send(new GetAllWorkersQuery()));
        }

        [HttpGet("by-ids/{userIds}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllWorkersWithIds(
            [FromRoute] string userIds, 
            [FromQuery] string separator = ",")
        {
            return Ok(await Mediator.Send(new GetAllWorkersWithIdsQuery()
            {
                UserIds = userIds,
                Separator = separator
            }));
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<WorkerDto>> GetWorkerData()
        {
            var worker = await Mediator.Send(new GetWorkerByIdQuery());

            return worker is null ? NotFound() : Ok(worker);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkerDto>> RegisterWorker([FromBody] RegisterWorkerDto registerDto)
        {
            var command = new RegisterWorkerCommand() { RegisterDto = registerDto };

            return Ok(await Mediator.Send(command));
        }

        [HttpPut()]
        [AuthorizeRoles(RoleType.Worker)]
        public async Task<ActionResult<WorkerDto>> UpdateWorker([FromBody] UpdateWorkerDto updateDto)
        {
            var command = new UpdateWorkerCommand() { UpdateDto = updateDto };

            return Ok(await Mediator.Send(command));
        }
    }
}
