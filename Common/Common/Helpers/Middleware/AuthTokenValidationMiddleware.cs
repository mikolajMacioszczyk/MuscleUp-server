using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Helpers.Middleware
{
    public class AuthTokenValidationMiddleware
    {
        private readonly IAuthorizationService _authService;
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthTokenValidationMiddleware> _logger;

        public AuthTokenValidationMiddleware(RequestDelegate next, IAuthorizationService authService, ILogger<AuthTokenValidationMiddleware> logger)
        {
            _next = next;
            _authService = authService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, HttpAuthContext httpAuthContext, IWebHostEnvironment environment)
        {
            if (context == null)
            {
                _logger.LogError("HttpContext is null");
                return;
            }

            if (httpAuthContext == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Error during authentication token validation");
                _logger.LogError("HttpAuthContext is null");
                return;
            }

            if (!AuthHelper.HasAuthorizationBarerToken(context.Request))
            {
                await _next(context);
                return;
            }

            var tokenString = AuthHelper.GetJwtString(context.Request);
            var jwtPayload = AuthHelper.GetJwtPayload(tokenString);

            switch (jwtPayload.TokenType)
            {
                case TokenType.None:
                case TokenType.AccessToken when RefreshTokenPath(context.Request):
                case TokenType.RefreshToken when !RefreshTokenPath(context.Request):
                    _logger.LogWarning("Access attempt with wrong token type");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Access attempt with wrong token type");
                    return;
            }

            httpAuthContext.MapFrom(jwtPayload);
            if (!HttpAuthContextValid(httpAuthContext))
            {
                _logger.LogWarning("Access attempt with invalid http auth context");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid http auth context");
                return;
            }

            var validationResult = await ValidateToken(context, httpAuthContext, jwtPayload, tokenString);
            switch (validationResult)
            {
                case TokenValidationResult.InternalError:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Error during authentication token validation");
                    _logger.LogError($"Validation {jwtPayload.TokenType} failed");
                    return;

                case TokenValidationResult.UnauthenticatedTokenInvalid:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid authentication token");
                    _logger.LogWarning($"Access attempt with invalidated {jwtPayload.TokenType}");
                    return;

                case TokenValidationResult.Valid:
                    jwtPayload.MapInto(context.Items);
                    // TODO: set application language if needed
                    await _next(context);
                    return;
                default:
                    throw new NotSupportedException(validationResult.ToString());
            }
        }

        private bool HttpAuthContextValid(HttpAuthContext httpAuthContext) => httpAuthContext.TokenType switch
        {
            TokenType.AccessToken => httpAuthContext.UserRole != RoleType.None && !string.IsNullOrEmpty(httpAuthContext.UserId),
            TokenType.RefreshToken => !string.IsNullOrEmpty(httpAuthContext.UserId),
            TokenType.None => false,
            _ => throw new NotSupportedException(httpAuthContext.TokenType.ToString())
        };

        private static bool RefreshTokenPath(HttpRequest httpRequest) => httpRequest.Path.Value.Contains(
            CommonConsts.RefreshTokenPath, StringComparison.InvariantCultureIgnoreCase);

        private async Task<TokenValidationResult> ValidateToken(HttpContext context, HttpAuthContext httpAuthContext, JwtPayload jwtPayload, string tokenString)
            => jwtPayload.TokenType switch
        {
            TokenType.AccessToken => await _authService.ValidateAuthToken(jwtPayload, false),
            TokenType.RefreshToken => await _authService.ValidateAuthToken(jwtPayload, true),
            _ => throw new NotSupportedException(jwtPayload.TokenType.ToString())
        };
    }
}
