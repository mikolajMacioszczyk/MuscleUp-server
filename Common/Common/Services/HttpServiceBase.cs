using Common.Consts;
using Common.Helpers;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Common.Services
{
    public abstract class HttpServiceBase
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpServiceBase> _logger;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        protected HttpServiceBase(IHttpClientFactory httpClientFactory, ILogger<HttpServiceBase> logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
        }

        protected abstract string BaseAddress { get; }

        protected async Task<Result<T>> SendGetRequestAsync<T>(string path)
        {
            EnsureHttpClientCreated();

            var cancellationToken = new CancellationToken();

            var request = new HttpRequestMessage(HttpMethod.Get, path);

            var context = _contextAccessor.HttpContext;
            if (AuthHelper.HasAuthorizationBarerToken(context.Request))
            {
                request.Headers.Add(HeaderNames.Authorization, $"{AuthConsts.BearerPrefix} {AuthHelper.GetJwtString(context.Request)}");
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                try
                {
                    var deserialized = JsonSerializer.Deserialize<T>(content, options);
                    return new Result<T>(deserialized);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    throw;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Result<T>(CommonConsts.NOT_FOUND);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new Result<T>(errorMessage);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"Unauthorized access to {path}");
            }
            throw new NotImplementedException();
        }

        private void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
            {
                _httpClient = _httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(BaseAddress);
            }
        }
    }
}
