using Common.Enums;
using Common.Models;

namespace Common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<TokenValidationResult> ValidateAuthToken(JwtPayload jwtPayload, bool isRefreshToken);
    }
}
