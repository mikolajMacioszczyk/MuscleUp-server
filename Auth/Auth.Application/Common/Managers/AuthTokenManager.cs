using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;

namespace Auth.Application.Common.Managers
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

        public async Task DeleteAsync(string userId)
        {
            _authTokenRepository.Delete(userId);
            await _authTokenRepository.SaveChangesAsync();
        }

        public AuthToken GetByAccessTokenId(string userId, Guid accessTokenId) =>
            _authTokenRepository.GetByAccessTokenId(userId, accessTokenId);

        public AuthToken GetByRefreshTokenId(string userId, Guid refreshTokenId) =>
            _authTokenRepository.GetByRefreshTokenId(userId, refreshTokenId);

        public AuthToken GetByUser(string userId) =>
            _authTokenRepository.GetById(userId);
    }
}
