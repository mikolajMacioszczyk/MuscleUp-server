using Auth.Application.Users.Queries;
using AutoMapper;
using Common.Attribute;
using Common.BaseClasses;
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
