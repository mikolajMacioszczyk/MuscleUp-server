using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IAuthTokenManager
    {
        Task UpdateAsync(AuthToken authToken);
        Task AddAsync(AuthToken authToken);
        Task DeleteAsync(Guid userId);
        AuthToken GetByUser(Guid userId);
        AuthToken GetByAccessTokenId(Guid userId, Guid accessTokenId);
        AuthToken GetByRefreshTokenId(Guid userId, Guid refreshTokenId);
    }
}
