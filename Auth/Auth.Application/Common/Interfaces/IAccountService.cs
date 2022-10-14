using Auth.Application.Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResult> Login(LoginRequest request, string userAgent);
        Task<LoginResult> LoginWithRefreshToken(bool rememberMe, string userAgent);
        Task Logout();

    }
}
