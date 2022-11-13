using Auth.Application.Common.Dtos;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models;
using Auth.Application.Dtos;
using Auth.Application.Members.Commands;
using Common.Attribute;
using Common.BaseClasses;
using Common.Extensions;
using Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountHttpManager _accountHttpManager;
        private readonly IUserService _userService;
        private readonly IFacebookLoginService _facebookLoginService;

        public AccountController(
            IAccountHttpManager accountHttpManager, 
            IUserService userService, 
            IFacebookLoginService facebookLoginService)
        {
            _accountHttpManager = accountHttpManager;
            _userService = userService;
            _facebookLoginService = facebookLoginService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await _accountHttpManager.Login(request, HttpContext.GetUserAgent()));
        }

        [AllowAnonymous]
        [HttpPost("login-with-facebook")]
        public async Task<ActionResult<AuthResponse>> LoginWithFacebook([FromBody] FacebookLoginViewModel request)
        {
            if (!(await _facebookLoginService.ValidateToken(request.AccessToken, request.UserId, request.Email)))
            {
                return Unauthorized();
            }

            // get user by provided Email
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                var command = new RegisterMemberFromExternalServiceCommand(request);
                // UserId MUST be the same as user from Facebook!
                var createdMember = await Mediator.Send(command);
                user = createdMember.User;
            }

            var authResponse = await _accountHttpManager.LoginExistingUser(user, HttpContext.GetUserAgent());
            return Ok(authResponse);
        }

        [AllowAnonymous]
        [HttpPost("login-with-refresh-token")]
        public async Task<ActionResult<AuthResponse>> LoginWithRefreshToken()
        {
            bool rememberMe = false;
            return Ok(await _accountHttpManager.LoginWithRefreshToken(rememberMe, HttpContext.GetUserAgent()));
        }

        [HttpPost("change-password")]
        [AuthorizeRoles(AuthHelper.RoleAllExceptAdmin)]
        public async Task<ActionResult<AuthResponse>> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            return Ok(await _userService.ChangePasswordAsync(request));
        }

        [AuthorizeRoles(AuthHelper.RoleAll)]
        [HttpPut("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountHttpManager.Logout();

            return NoContent();
        }
    }
}
