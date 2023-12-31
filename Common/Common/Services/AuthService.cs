﻿using Common.Enums;
using Common.Interfaces;
using Common.Models;

namespace Common.Services
{
    public class AuthService : IAuthorizationService
    {
        public Task<TokenValidationResult> ValidateAuthToken(JwtPayload jwtPayload, bool isRefreshToken)
        {
            if (jwtPayload == null) throw new ArgumentNullException(nameof(jwtPayload));

            // TODO: Implement

            return Task.FromResult(TokenValidationResult.Valid);
        }
    }
}
