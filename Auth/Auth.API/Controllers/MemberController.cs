using Auth.Application.Members.Commands;
using Auth.Application.Members.Dtos;
using Auth.Application.Members.Queries;
using Common.BaseClasses;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    public class MemberController : ApiControllerBase
    {
        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleType.Administrator))]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            return Ok(await Mediator.Send(new GetAllMembersQuery()));
        }

        [HttpGet()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<MemberDto>> GetMemeberData()
        {
            var member = await Mediator.Send(new GetMemberByIdQuery());

            return member is null ? NotFound() : Ok(member);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<MemberDto>> RegisterMember([FromBody] RegisterMemberDto registerDto)
        {
            var command = new RegisterMemberCommand() { RegisterDto = registerDto };

            return Ok(await Mediator.Send(command));
        }

        [HttpPut()]
        [Authorize(Roles = nameof(RoleType.Member))]
        public async Task<ActionResult<MemberDto>> UpdateMember([FromBody] UpdateMemberDto updateDto)
        {
            var command = new UpdateMemberCommand() { UpdateDto = updateDto };

            return Ok(await Mediator.Send(command));
        }
    }
}
