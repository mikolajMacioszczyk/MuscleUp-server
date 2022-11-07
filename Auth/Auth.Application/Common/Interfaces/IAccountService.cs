using Auth.Application.Common.Models;
using Auth.Domain.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResult> Login(LoginRequest request, string userAgent);
        Task<LoginResult> Login(ApplicationUser user, string userAgent);
        Task<LoginResult> LoginWithRefreshToken(bool rememberMe, string userAgent);
        Task Logout();

    }
}
