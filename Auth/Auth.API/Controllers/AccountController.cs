using Auth.Application.Common.Dtos;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models;
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

        public AccountController(IAccountHttpManager accountHttpManager, IUserService userService)
        {
            _accountHttpManager = accountHttpManager;
            _userService = userService;
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
        [HttpPost("login-with-refresh-token")]
        public async Task<ActionResult<AuthResponse>> LoginWithRefreshToken()
        {
            bool rememberMe = false;
            return Ok(await _accountHttpManager.LoginWithRefreshToken(rememberMe, HttpContext.GetUserAgent()));
        }

        [HttpPost("change-password")]
        [Authorize(Roles = AuthHelper.RoleAllExceptAdmin)]
        public async Task<ActionResult<AuthResponse>> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            return Ok(await _userService.ChangePasswordAsync(request));
        }

        [Authorize(Roles = AuthHelper.RoleAll)]
        [HttpPut("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountHttpManager.Logout();

            return NoContent();
        }
    }
}
