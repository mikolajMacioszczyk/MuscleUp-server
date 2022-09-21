using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Common.Consts;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Domain.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthTokenManager _authTokenManager;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly HttpAuthContext _httpAuthContext;

        public AuthTokenService(
            IApplicationUserManager applicationUserManager,
            IOptions<JwtOptions> jwtOptions,
            UserManager<ApplicationUser> userManager,
            HttpAuthContext httpAuthContext,
            JwtSecurityTokenHandler jwtTokenHandler, 
            IAuthTokenManager authTokenManager)
        {
            _applicationUserManager = applicationUserManager;
            _jwtOptions = jwtOptions;
            _userManager = userManager;
            _httpAuthContext = httpAuthContext;
            _jwtTokenHandler = jwtTokenHandler;
            _authTokenManager = authTokenManager;
        }

        public async Task<(string AccessToken, string RefreshToken)> CreateAuthToken(ApplicationUser user)
        {
            if (user is null) return default;

            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthConsts.JwtSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            // TODO: Handle user settings here if needed

            var accessToken = await CreateAccessToken(user, now, signingCredentials);
            var accessTokenString = _jwtTokenHandler.WriteToken(accessToken);

            var refreshToken = await CreateRefreshToken(user, now, signingCredentials);
            var refreshTokenString = _jwtTokenHandler.WriteToken(refreshToken);

            var existingAuthToken = _authTokenManager.GetByUser(Guid.Parse(user.Id));
            if (existingAuthToken != null)
            {
                await _authTokenManager.UpdateAsync(PopulateAuthToken(existingAuthToken));
            }
            else
            {
                await _authTokenManager.AddAsync(PopulateAuthToken(new AuthToken()));
            }

            return (accessTokenString, refreshTokenString);

            AuthToken PopulateAuthToken(AuthToken token)
            {
                token.Id = Guid.Parse(user.Id);
                token.AccessTokenId = Guid.Parse(accessToken.Id);
                token.AccessTokenExpiration = now.AddMinutes(_jwtOptions.Value.AccessExpirationMinutes);
                token.RefreshTokenId = Guid.Parse(refreshToken.Id);
                token.RefreshTokenExpiration = now.AddMinutes(_jwtOptions.Value.RefreshExpirationMinutes);

                return token;
            }
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshAuthToken(string userId)
        {
            var user = await _applicationUserManager.GetById(userId.ToString());

            return await CreateAuthToken(user);
        }

        public async Task RemoveAuthToken(string userId)
        {
            await _authTokenManager.DeleteAsync(Guid.Parse(userId));
        }

        private async Task<JwtSecurityToken> CreateAccessToken(
            ApplicationUser user,
            DateTime now,
            SigningCredentials signingCredentials) => new JwtSecurityToken(
                claims: await GetClaims(user),
                expires: now.AddMinutes(_jwtOptions.Value.AccessExpirationMinutes),
                signingCredentials: signingCredentials)
            {
                Payload =
                {
                    [JwtPayloadKey.UserId] = user.Id,
                    [JwtPayloadKey.FirstName] = user.FirstName,
                    [JwtPayloadKey.LastName] = user.LastName,
                    [JwtPayloadKey.TokenType] = TokenType.AccessToken.ToString(),
                }
            };

        private async Task<JwtSecurityToken> CreateRefreshToken(
            ApplicationUser user,
            DateTime now,
            SigningCredentials signingCredentials) => new JwtSecurityToken(
                claims: await GetClaims(user),
                expires: now.AddMinutes(_jwtOptions.Value.RefreshExpirationMinutes),
                signingCredentials: signingCredentials)
            {
                Payload =
                {
                    [JwtPayloadKey.UserId] = user.Id,
                    [JwtPayloadKey.TokenType] = TokenType.RefreshToken.ToString(),
                }
            };

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtPayloadKey.Sub, user.UserName),
                new Claim(JwtPayloadKey.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(JwtPayloadKey.Role, role)));

            return claims;
        }
    }
}
