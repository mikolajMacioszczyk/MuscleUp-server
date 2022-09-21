using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IAuthTokenRepository
    {
        void Delete(Guid UserId);
        AuthToken GetById(Guid UserId);
        AuthToken GetByAccessTokenId(Guid UserId, Guid accessTokenId);
        AuthToken GetByRefreshTokenId(Guid UserId, Guid refreshTokenId);
        AuthToken Update(AuthToken authToken);
        AuthToken Add(AuthToken authToken);
        Task SaveChangesAsync();
    }
}
