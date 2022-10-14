using Auth.Application.Common.Dtos;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models;
using Auth.Domain.Models;
using Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Common.Services
{
    public class UserService : IUserService
    {
        private readonly HttpAuthContext _httpAuthContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthTokenService _authTokenService;
        protected readonly IUserStore<ApplicationUser> _userStore;

        public UserService(
            HttpAuthContext httpAuthContext,
            UserManager<ApplicationUser> userManager,
            IAuthTokenService authTokenService,
            IUserStore<ApplicationUser> userStore)
        {
            _httpAuthContext = httpAuthContext;
            _userManager = userManager;
            _authTokenService = authTokenService;
            _userStore = userStore;
        }

        public async Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            if (request == null) throw new ArgumentException(nameof(request));

            var applicationUser = await _userManager.FindByIdAsync(_httpAuthContext.UserId);
            if (applicationUser is null)
            {
                throw new UnauthorizedAccessException();
            }

            if (!await _userManager.CheckPasswordAsync(applicationUser, request.OldPassword))
            {
                throw new Exception($"Changing password failed: incorrect password");
            }

            applicationUser.UserName = await _userStore.GetUserNameAsync(applicationUser, CancellationToken.None);

            var result = await _userManager.ChangePasswordAsync(applicationUser, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"Changing password failed: {string.Join(", ", result.Errors)}");
            }

            var authReponse = new AuthResponse();
            (authReponse.AccessToken, authReponse.RefreshToken) = await _authTokenService.CreateAuthToken(applicationUser);
            return authReponse;
        }
    }
}
