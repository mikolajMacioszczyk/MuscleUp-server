using Common.Enums;
using Common.Interfaces;
using Common.Models;

namespace Common.Services
{
    public class AuthService : IAuthService
    {
        public async Task<TokenValidationResult> ValidateAuthToken(JwtPayload jwtPayload, bool isRefreshToken)
        {
            if (jwtPayload == null) throw new ArgumentNullException(nameof(jwtPayload));

            // TODO: Implement

            return TokenValidationResult.Valid;
        }
    }
}
