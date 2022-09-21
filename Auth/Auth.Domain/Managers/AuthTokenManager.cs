using Auth.Domain.Interfaces;
using Auth.Domain.Models;

namespace Auth.Domain.Managers
{
    public class AuthTokenManager : IAuthTokenManager
    {
        private readonly IAuthTokenRepository _authTokenRepository;

        public AuthTokenManager(IAuthTokenRepository authTokenRepository)
        {
            _authTokenRepository = authTokenRepository;
        }

        public async Task AddAsync(AuthToken authToken)
        {
            _authTokenRepository.Add(authToken);
            await _authTokenRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(AuthToken authToken)
        {
            _authTokenRepository.Update(authToken);
            await _authTokenRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            _authTokenRepository.Delete(userId);
            await _authTokenRepository.SaveChangesAsync();
        }

        public AuthToken GetByAccessTokenId(Guid userId, Guid accessTokenId) =>
            _authTokenRepository.GetByAccessTokenId(userId, accessTokenId);

        public AuthToken GetByRefreshTokenId(Guid userId, Guid refreshTokenId) =>
            _authTokenRepository.GetByRefreshTokenId(userId, refreshTokenId);

        public AuthToken GetByUser(Guid userId) =>
            _authTokenRepository.GetById(userId);
    }
}
