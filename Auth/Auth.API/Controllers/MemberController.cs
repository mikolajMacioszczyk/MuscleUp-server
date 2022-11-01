using Auth.Application.Members.Commands;
using Auth.Application.Members.Dtos;
using Auth.Application.Members.Queries;
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
    public class MemberController : ApiControllerBase
    {
        private readonly HttpAuthContext _httpAuthContext;

        public MemberController(HttpAuthContext httpAuthContext)
        {
            _httpAuthContext = httpAuthContext;
        }

        [HttpGet("all")]
        [AuthorizeRoles(RoleType.Administrator)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            return Ok(await Mediator.Send(new GetAllMembersQuery()));
        }

        [HttpGet("by-ids/{userIds}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembersWithIds(
            [FromRoute] string userIds,
            [FromQuery] string separator = ",")
        {
            return Ok(await Mediator.Send(new GetAllMembersWithIdsQuery()
            {
                UserIds = userIds,
                Separator = separator
            }));
        }

        [HttpGet()]
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<MemberDto>> GetMemeberData()
        {
            var memberId = _httpAuthContext.UserId;
            var member = await Mediator.Send(new GetMemberByIdQuery(memberId));

            return member is null ? NotFound() : Ok(member);
        }

        [HttpGet("find/{userId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<MemberDto>> GetMemeberById([FromRoute] string userId)
        {
            var member = await Mediator.Send(new GetMemberByIdQuery(userId));

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
        [AuthorizeRoles(RoleType.Member)]
        public async Task<ActionResult<MemberDto>> UpdateMember([FromBody] UpdateMemberDto updateDto)
        {
            var command = new UpdateMemberCommand() { UpdateDto = updateDto };

            return Ok(await Mediator.Send(command));
        }
    }
}
