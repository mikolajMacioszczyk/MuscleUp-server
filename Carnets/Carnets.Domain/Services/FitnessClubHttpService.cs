using Carnets.Domain.Interfaces;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Carnets.Domain.Services
{
    public class FitnessClubHttpService : HttpServiceBase, IFitnessClubHttpService
    {
        private readonly string _baseAddress;
        protected override string BaseAddress => _baseAddress;

        public FitnessClubHttpService(
            IHttpClientFactory httpClientFactory, 
            ILogger<HttpServiceBase> logger,
            IConfiguration configuration) 
            : base(httpClientFactory, logger)
        {
            _baseAddress = configuration.GetSection("Api").GetValue<string>("FitnessClubHost");
        }

        public async Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId)
        {
            return await SendGetRequestAsync<FitnessClubDto>($"fitnessClub/worker/{workerId}");
        }

        public async Task<Result<FitnessClubDto>> EnsureWorkerCanManageFitnessClub(string workerId)
        {
            var fitnessClubResult = await GetFitnessClubOfWorker(workerId);
            if (!fitnessClubResult.IsSuccess)
            {
                throw new BadRequestException("Cannot determine worker's fitness club");
            }
            return fitnessClubResult;
        }
    }
}
