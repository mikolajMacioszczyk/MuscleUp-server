using Common.Models;
using Common.Models.Dtos;
using Common.Services;
using FitnessClubs.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitnessClubs.Application.Services
{
    public class AuthHttpService : HttpServiceBase, IAuthService
    {
        public static readonly char Separator = ',';

        private readonly string _baseAddress;
        protected override string BaseAddress => _baseAddress;

        public AuthHttpService(
            IHttpClientFactory httpClientFactory, 
            ILogger<HttpServiceBase> logger, 
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration) 
            : base(httpClientFactory, logger, contextAccessor)
        {
            _baseAddress = configuration.GetSection("Api").GetValue<string>("AuthHost");
        }

        public async Task<Result<IEnumerable<MemberDto>>> GetAllMembersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || (!userIds.Any()))
            {
                return new Result<IEnumerable<MemberDto>>(Array.Empty<MemberDto>());
            }

            return await SendGetRequestAsync<IEnumerable<MemberDto>>("member/by-ids/" + JoinUserIds(userIds));
        }

        public async Task<Result<IEnumerable<TrainerDto>>> GetAllTrainersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || (!userIds.Any()))
            {
                return new Result<IEnumerable<TrainerDto>>(Array.Empty<TrainerDto>());
            }

            return await SendGetRequestAsync<IEnumerable<TrainerDto>>("trainer/by-ids/" + JoinUserIds(userIds));
        }

        public async Task<Result<IEnumerable<WorkerDto>>> GetAllWorkersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || (!userIds.Any()))
            {
                return new Result<IEnumerable<WorkerDto>>(Array.Empty<WorkerDto>());
            }

            return await SendGetRequestAsync<IEnumerable<WorkerDto>>("worker/by-ids/" + JoinUserIds(userIds));
        }

        private string JoinUserIds(IEnumerable<string> userIds)
        {
            return string.Join(Separator, userIds);
        }
    }
}
