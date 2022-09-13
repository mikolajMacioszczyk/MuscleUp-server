using Common.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Common.Services
{
    public abstract class HttpServiceBase
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpServiceBase> _logger;
        private HttpClient _httpClient;

        protected HttpServiceBase(IHttpClientFactory httpClientFactory, ILogger<HttpServiceBase> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected abstract string BaseAddress { get; }

        protected async Task<Result<T>> SendGetRequestAsync<T>(string path)
        {
            EnsureHttpClientCreated();

            var cancellationToken = new CancellationToken();

            var response = await _httpClient.GetAsync(path, cancellationToken);

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
