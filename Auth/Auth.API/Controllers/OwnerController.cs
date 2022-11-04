using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.Owners.Dtos;
using Auth.Application.Owners.Queries;
using Auth.Application.Owners.Commands;

namespace Auth.API.Controllers
{
    public class OwnerController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;

        public OwnerController(HttpAuthContext httpAuthContext)
        {
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("all")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
        {
            return Ok(await Mediator.Send(new GetAllOwnersQuery()));
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Owner)]
        public async Task<ActionResult<OwnerDto>> GetOwnerData()
        {
            var ownerId = _httpAuthContext.UserId;
            var owner = await Mediator.Send(new GetOwnerByIdQuery(ownerId));

           return owner is null ? NotFound() : Ok(owner);
        }

        [HttpGet("find/{userId}")]
        [AuthorizeRoles(RoleType.Owner, RoleType.Administrator)]
        public async Task<ActionResult<OwnerDto>> GetOwnerById([FromRoute] string userId)
        {
            var owner = await Mediator.Send(new GetOwnerByIdQuery(userId));

            return owner is null ? NotFound() : Ok(owner);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<OwnerDto>> RegisterOwner([FromBody] RegisterOwnerDto registerDto)
        {
            var command = new RegisterOwnerCommand(registerDto);
            
            return Ok(await Mediator.Send(command));
        }

        [HttpPut()]
        [AuthorizeRoles(RoleType.Owner)]
        public async Task<ActionResult<OwnerDto>> UpdateOwner([FromBody] UpdateOwnerDto updateDto)
        {
            var ownerId = _httpAuthContext.UserId;
            var command = new UpdateOwnerCommand(ownerId, updateDto);

            return Ok(await Mediator.Send(command));
        }
    }
}
