﻿using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models;
using Auth.Domain.Models;
using Common.Exceptions;
using Common.Models;

namespace Auth.Application.Common.Managers
{
    public class AccountHttpManager : IAccountHttpManager
    {
        private readonly IAccountService _accountService;
        private readonly HttpAuthContext _httpAuthContext;

        public AccountHttpManager(IAccountService accountService, HttpAuthContext httpAuthContext)
        {
            _accountService = accountService;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<AuthResponse> Login(LoginRequest request, string userAgent)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = await _accountService.Login(request, userAgent);
            ValidateResult(result, request.Email);

            return CreateResponse(result);
        }

        public async Task<AuthResponse> LoginExistingUser(ApplicationUser applicationUser, string userAgent)
        {
            if (applicationUser is null) throw new ArgumentException(nameof(applicationUser));

            var result = await _accountService.Login(applicationUser, userAgent);
            ValidateResult(result, applicationUser.Email);

            return CreateResponse(result);
        }

        public async Task<AuthResponse> LoginWithRefreshToken(bool rememberMe, string userAgent)
        {
            var result = await _accountService.LoginWithRefreshToken(rememberMe, userAgent);
            ValidateResult(result, _httpAuthContext.UserId);

            return CreateResponse(result);
        }

        public async Task Logout()
        {
            await _accountService.Logout();
        }

        private static void ValidateResult(LoginResult result, object logData)
        {
            if (result.UserNotFound)
            {
                throw new InvalidInputException($"Username doesn't exists. {logData}");
            }
            if (result.IsInvalidPassword)
            {
                throw new InvalidInputException($"Invalid password. {logData}");
            }
        }

        private static AuthResponse CreateResponse(LoginResult result)
        {
            return new AuthResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            };
        }
    }
}
