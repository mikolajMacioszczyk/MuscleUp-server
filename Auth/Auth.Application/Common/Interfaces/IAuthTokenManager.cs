using Auth.Domain.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IAuthTokenManager
    {
        Task UpdateAsync(AuthToken authToken);
        Task AddAsync(AuthToken authToken);
        Task DeleteAsync(string userId);
        AuthToken GetByUser(string userId);
        AuthToken GetByAccessTokenId(string userId, Guid accessTokenId);
        AuthToken GetByRefreshTokenId(string userId, Guid refreshTokenId);
    }
}
