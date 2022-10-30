using Carnets.Application.Consts;
using Carnets.Application.Interfaces;
using Carnets.Application.Models;
using Common.Consts;
using Common.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Carnets.Application.Services
{
    public class EntryTokenService : IEntryTokenService
    {
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly string _secret;
        private readonly int _entryTokenValidityInSeconds;

        public EntryTokenService(
            IConfiguration configuration, 
            JwtSecurityTokenHandler jwtTokenHandler)
        {
            _secret = configuration.GetValue<string>(AuthConsts.JwtSecretKey);
            _entryTokenValidityInSeconds = configuration.GetValue<int>(CarnetsConsts.EntryTokenValidityInSecondsKey);
            _jwtTokenHandler = jwtTokenHandler;
        }

        public string GenerateToken(string gympassId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var expires = DateTime.UtcNow.AddSeconds(_entryTokenValidityInSeconds);

            var entryToken = CreateEntryToken(gympassId, expires, signingCredentials);
            var entryTokenString = _jwtTokenHandler.WriteToken(entryToken);

            return entryTokenString;
        }

        public EntryTokenPayload DecodeToken(string entryToken)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(entryToken);
            var gympassIdString = GetPayloadValue<string>(CarnetsConsts.GympassIdPayloadKey);
            var expiresDate = GetPayloadValue<DateTime>(CarnetsConsts.ExpiresDatePayloadKey);

            T GetPayloadValue<T>(string key) => jwtToken.Payload.ContainsKey(key) ? (T)jwtToken.Payload[key] : default;

            return new EntryTokenPayload
            {
                GympassId = gympassIdString,
                ExpiresDate = expiresDate,
            };
        }

        private JwtSecurityToken CreateEntryToken(
            string gympassId,
            DateTime expires,
            SigningCredentials signingCredentials) => new JwtSecurityToken(
                claims: GetEntryTokenMemberClaims(gympassId),
                expires: expires,
                signingCredentials: signingCredentials)
            {
                Payload =
                {
                    [CarnetsConsts.GympassIdPayloadKey] = gympassId,
                    [CarnetsConsts.ExpiresDatePayloadKey] = expires,
                }
            };

        private List<Claim> GetEntryTokenMemberClaims(string gympassId)
        {
            var claims = new List<Claim>()
            {
                new Claim(CarnetsConsts.GympassIdPayloadKey, gympassId),
                new Claim(JwtPayloadKey.Jti, Guid.NewGuid().ToString()),
            };

            return claims;
        }
    }
}
