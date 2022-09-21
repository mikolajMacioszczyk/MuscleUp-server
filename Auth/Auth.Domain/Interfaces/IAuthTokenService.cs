﻿using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IAuthTokenService
    {
        Task<(string AccessToken, string RefreshToken)> CreateAuthToken(ApplicationUser user);
        Task<(string AccessToken, string RefreshToken)> RefreshAuthToken(string userId);
        Task RemoveAuthToken(string userId);
    }
}
