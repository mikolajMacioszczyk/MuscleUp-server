using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;

namespace Auth.Repo.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        private readonly AuthDbContext _context;

        public AuthTokenRepository(AuthDbContext context)
        {
            _context = context;
        }

        public AuthToken GetById(Guid UserId) =>
            _context.AuthTokens.SingleOrDefault(t => t.Id == UserId);

        public AuthToken GetByAccessTokenId(Guid UserId, Guid accessTokenId) =>
            _context.AuthTokens.SingleOrDefault(t => t.Id == UserId && t.AccessTokenId == accessTokenId);

        public AuthToken GetByRefreshTokenId(Guid UserId, Guid refreshTokenId) =>
            _context.AuthTokens.SingleOrDefault(t => t.Id == UserId && t.RefreshTokenId == refreshTokenId);

        public AuthToken Add(AuthToken authToken) =>
            _context.Add(authToken).Entity;

        public AuthToken Update(AuthToken authToken) =>
            _context.Update(authToken).Entity;

        public void Delete(Guid UserId)
        {
            var toDelete = _context.AuthTokens.Where(t => t.Id == UserId);
            _context.AuthTokens.RemoveRange(toDelete);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
