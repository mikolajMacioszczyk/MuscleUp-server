using Common.Enums;
using Common.Models;

namespace Common.Interfaces
{
    public interface IAuthService
    {
        Task<TokenValidationResult> ValidateAuthToken(JwtPayload jwtPayload, bool isRefreshToken);
    }
}
