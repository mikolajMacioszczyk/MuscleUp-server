using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models;
using Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Auth.Application.Common.Services
{
    public class FacebookOnlineLoginService : IFacebookLoginService
    {
        private readonly ILogger<FacebookOnlineLoginService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _appId;
        private readonly string _appSecret;

        private const string ValidationBasePath = "https://graph.facebook.com/debug_token";
        private const string InputTokenKey = "input_token";
        private const string AccessTokenKey = "access_token";

        public FacebookOnlineLoginService(
            ILogger<FacebookOnlineLoginService> logger, 
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;

            var facebookSection = configuration.GetSection("Facebook");
            _appId = facebookSection.GetValue<string>("AppId");
            _appSecret = facebookSection.GetValue<string>("AppSecret");
        }

        public async Task<bool> ValidateToken(string accessToken, string userId, string email)
        {
            var now = DateTime.UtcNow;
            try
            {
                var (isSuccess, contentString) = await SendVerificationRequest(accessToken);

                if (isSuccess)
                {
                    var result = JsonSerializer.Deserialize<FacebookVerificationResponse>(contentString);

                    if (result?.data != null &&
                        result.data.app_id == _appId &&
                        result.data.is_valid &&
                        result.data.user_id == userId &&
                        CheckTokenNotExpired(result.data.expires_at, now))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception when validating facebbok token ({accessToken}).\nError: {ex.Message}");
                throw new ArgumentException($"Exception when validating facebbok token");
            }
        }

        private static bool CheckTokenNotExpired(double unixTimeStamp, DateTime now)
        {
            // Unix timestamp is seconds past epoch
            DateTime expirationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            expirationDate = expirationDate.AddSeconds(unixTimeStamp).ToLocalTime();

            return expirationDate.ToUniversalTime() > now.ToUniversalTime();
        }

        private async Task<(bool isSuccess, string content)> SendVerificationRequest(string accessToken)
        {
            var uri = UriHelper.AppendQueryParamToUri(ValidationBasePath, InputTokenKey, accessToken);
            uri = UriHelper.AppendQueryParamToUri(uri, AccessTokenKey, $"{_appId}|{_appSecret}");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

                return (true, contentString);
            }
            return (false, string.Empty);
        }
    }
}
