using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IAccountHttpManager
    {
        Task<AuthResponse> Login(LoginRequest request, string userAgent);

        Task<AuthResponse> LoginWithRefreshToken(bool rememberMe, string userAgent);
        Task Logout();
    }
}
