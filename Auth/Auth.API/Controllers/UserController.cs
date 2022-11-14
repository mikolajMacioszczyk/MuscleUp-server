using Auth.Application.Users.Queries;
using Auth.Domain.Models;
using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
using Common.Enums;
using Common.Helpers;
using Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{email}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<AnyUserDto>> GetUserByEmail([FromRoute] string email)
        {
            var (user, userRole) = await Mediator.Send(new GetUserByEmailQuery(email));

            return MapToAnyUserDto(user, userRole);
        }

        [HttpGet("by-id/{userId}")]
        [AuthorizeRoles(AuthHelper.RoleAll)]
        public async Task<ActionResult<AnyUserDto>> GetUserById([FromRoute] string userId)
        {
            var (user, userRole) = await Mediator.Send(new GetUserByIdQuery(userId));

            return MapToAnyUserDto(user, userRole);
        }

        private ActionResult<AnyUserDto> MapToAnyUserDto(ApplicationUser user, RoleType userRole)
        {
            if (user is null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<AnyUserDto>(user);
            dto.UserRole = userRole;

            return Ok(dto);
        }
    }
}
