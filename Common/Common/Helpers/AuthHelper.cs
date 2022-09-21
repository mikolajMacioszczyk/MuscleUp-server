using Common.Consts;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace Common.Helpers
{
    public static class AuthHelper
    {
        public const string RoleAll = nameof(RoleType.Member) + "," + nameof(RoleType.Trainer) + "," + nameof(RoleType.Worker) + "," + nameof(RoleType.Administrator);
        public const string RoleAllExceptAdmin = nameof(RoleType.Member) + "," + nameof(RoleType.Trainer) + "," + nameof(RoleType.Worker);
        public static bool HasAuthorizationBarerToken(HttpRequest request)
        {
            return request.Headers.ContainsKey(HeaderNames.Authorization)
                && !string.IsNullOrEmpty(request.Headers[HeaderNames.Authorization])
                && request.Headers[HeaderNames.Authorization].First().Length > AuthConsts.BearerPrefix.Length
                && !string.IsNullOrEmpty(request.Headers[HeaderNames.Authorization].First()[AuthConsts.BearerPrefix.Length..].Trim());
        }

        public static string GetJwtString(HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return request.Headers[HeaderNames.Authorization].First()[AuthConsts.BearerPrefix.Length..].Trim();
        }

        public static Models.JwtPayload GetJwtPayload(string tokenString)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
            var userIdString = GetPayloadValue<string>(JwtPayloadKey.UserId);
            var userRoleString = GetPayloadValue<string>(JwtPayloadKey.Role);
            var tokenIdString = GetPayloadValue<string>(JwtPayloadKey.Jti);
            var tokenTypeString = GetPayloadValue<string>(JwtPayloadKey.TokenType);

            return new Models.JwtPayload()
            {
                UserId = userIdString,
                Role = string.IsNullOrEmpty(userRoleString) ? RoleType.None : Enum.Parse<RoleType>(userRoleString),
                Jti = string.IsNullOrEmpty(tokenIdString) ? Guid.Empty : Guid.Parse(tokenIdString),
                TokenType = string.IsNullOrEmpty(tokenTypeString) ? TokenType.None : Enum.Parse<TokenType>(tokenTypeString),
            };

            T GetPayloadValue<T>(string key) => jwtToken.Payload.ContainsKey(key) ? (T)jwtToken.Payload[key] : default;
        }
    }
}
