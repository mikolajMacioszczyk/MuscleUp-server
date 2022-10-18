using Carnets.Application.Interfaces;
using Common.Models;
using Common.Models.Dtos;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Services
{
    public class FitnessClubHttpService : HttpServiceBase, IFitnessClubHttpService
    {
        private readonly string _baseAddress;
        protected override string BaseAddress => _baseAddress;

        public FitnessClubHttpService(
            IHttpClientFactory httpClientFactory,
            ILogger<HttpServiceBase> logger,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
            : base(httpClientFactory, logger, contextAccessor)
        {
            _baseAddress = configuration.GetSection("Api").GetValue<string>("FitnessClubHost");
        }

        public Task<Result<FitnessClubDto>> GetFitnessClubById(string fitnessClubId)
        {
            return SendGetRequestAsync<FitnessClubDto>($"fitness-club/{fitnessClubId}");
        }

        public Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId)
        {
            return SendGetRequestAsync<FitnessClubDto>($"fitness-club/worker/{workerId}");
        }
    }
}
