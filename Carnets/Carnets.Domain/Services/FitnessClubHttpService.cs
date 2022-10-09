using Carnets.Domain.Interfaces;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using Common.Services;
using Microsoft.AspNetCore.Http;
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

        public async Task<FitnessClubDto> EnsureWorkerCanManageFitnessClub(string workerId)
        {
            var fitnessClubResult = await GetFitnessClubOfWorker(workerId);
            if (!fitnessClubResult.IsSuccess)
            {
                throw new BadRequestException("Cannot determine worker's fitness club");
            }
            return fitnessClubResult.Value;
        }

        public async Task<FitnessClubDto> EnsureFitnessClubExists(string fitnessClubId)
        {
            var fitnessClubResult = await GetFitnessClubById(fitnessClubId);
            if (!fitnessClubResult.IsSuccess)
            {
                throw new BadRequestException($"Fitness club with id {fitnessClubId} does not exists");
            }
            return fitnessClubResult.Value;
        }
    }
}
