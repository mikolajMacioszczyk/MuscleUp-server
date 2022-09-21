using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResult> Login(LoginRequest request, string userAgent);
        Task<LoginResult> LoginWithRefreshToken(bool rememberMe, string userAgent);
        Task Logout();

    }
}
