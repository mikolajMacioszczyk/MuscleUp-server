using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Auth.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly HttpAuthContext _authContext;

        public AccountService(
            IApplicationUserManager applicationUserManager,
            IAuthTokenService authTokenService,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountService> logger,
            HttpAuthContext authContext)
        {
            _applicationUserManager = applicationUserManager;
            _authTokenService = authTokenService;
            _userManager = userManager;
            _logger = logger;
            _authContext = authContext;
        }

        public async Task<LoginResult> Login(LoginRequest request, string userAgent)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var (user, result) = await GetUserByEmail(request.Email);

            if (result.UserNotFound)
            {
                return result;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                result.IsInvalidPassword = true;
                return result;
            }

            await CreateAndAssignAuthToken(result, () => _authTokenService.CreateAuthToken(user));

            LogLoginIfSuccessful(result, user);

            return result;
        }

        public async Task<LoginResult> LoginWithRefreshToken(bool rememberMe, string userAgent)
        {
            var (user, result) = await GetUserById(_authContext.UserId);
            if (result.UserNotFound)
            {
                return result;
            }

            await CreateAndAssignAuthToken(result, () => _authTokenService.RefreshAuthToken(_authContext.UserId));

            LogLoginIfSuccessful(result, user);

            return result;
        }

        public async Task Logout()
        {
            _logger.LogInformation($"User {_authContext.UserId} logged out");

            await _authTokenService.RemoveAuthToken(_authContext.UserId);
        }

        private async Task<(ApplicationUser User, LoginResult Result)> GetUserByEmail(string email)
        {
            var user = await _applicationUserManager.GetByEmail(email);
            var result = new LoginResult()
            {
                UserNotFound = user is null
            };

            return (user, result);
        }

        private async Task<(ApplicationUser User, LoginResult Result)> GetUserById(string userId)
        {
            var user = await _applicationUserManager.GetById(userId);
            var result = new LoginResult()
            {
                UserNotFound = user is null
            };

            return (user, result);
        }

        private async Task CreateAndAssignAuthToken(
            LoginResult result,
            Func<Task<(string AccessToken, string RefreshToken)>> tokenFactory)
        {
            var (accessToken, refreshToken) = await tokenFactory();
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
        }

        private void LogLoginIfSuccessful(LoginResult result, ApplicationUser user)
        {
            if (!string.IsNullOrEmpty(result.AccessToken) || !string.IsNullOrEmpty(result.RefreshToken))
            {
                _logger.LogInformation($"User '{user}' ({user.Id}) logged in");
            }
        }
    }
}
