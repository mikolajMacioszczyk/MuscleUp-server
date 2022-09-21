using Auth.Domain.Dtos;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Common.Enums;
using Common.Extensions;
using Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
        [HttpPost("loginWithRefreshToken")]
        public async Task<ActionResult<AuthResponse>> LoginWithRefreshToken()
        {
            bool rememberMe = false;
            return Ok(await _accountHttpManager.LoginWithRefreshToken(rememberMe, HttpContext.GetUserAgent()));
        }

        [HttpPost("changePassword")]
        [Authorize(Roles = AuthHelper.RoleAllExceptAdmin)]
        public async Task<ActionResult<AuthResponse>> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            return Ok(await _userService.ChangePasswordAsync(request));
        }

        [Authorize(Roles = AuthHelper.RoleAll)]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountHttpManager.Logout();

            return Ok();
        }
    }
}
