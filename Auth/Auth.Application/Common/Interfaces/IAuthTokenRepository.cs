using Auth.Domain.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IAuthTokenRepository
    {
        void Delete(string UserId);
        AuthToken GetById(string UserId);
        AuthToken GetByAccessTokenId(string UserId, Guid accessTokenId);
        AuthToken GetByRefreshTokenId(string UserId, Guid refreshTokenId);
        AuthToken Update(AuthToken authToken);
        AuthToken Add(AuthToken authToken);
        Task SaveChangesAsync();
    }
}
