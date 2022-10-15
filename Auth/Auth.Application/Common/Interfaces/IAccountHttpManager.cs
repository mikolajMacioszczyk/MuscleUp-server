using Auth.Application.Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IAccountHttpManager
    {
        Task<AuthResponse> Login(LoginRequest request, string userAgent);

        Task<AuthResponse> LoginWithRefreshToken(bool rememberMe, string userAgent);
        Task Logout();
    }
}
