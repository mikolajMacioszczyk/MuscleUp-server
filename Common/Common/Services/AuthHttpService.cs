using Common.Exceptions;
using Common.Interfaces;
using Common.Models;
using Common.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Services
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

        public async Task<AnyUserDto> GetUserByEmail(string email)
        {
            if (email is null) throw new ArgumentException(nameof(email));

            var userResult = await SendGetRequestAsync<AnyUserDto>($"user/{email}");

            if (userResult.IsSuccess && userResult.Value != null)
            {
                return userResult.Value;
            }

            return null;
        }

        public async Task<Result<IEnumerable<MemberDto>>> GetAllMembersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || !userIds.Any())
            {
                return new Result<IEnumerable<MemberDto>>(Array.Empty<MemberDto>());
            }

            return await SendGetRequestAsync<IEnumerable<MemberDto>>("member/by-ids/" + JoinUserIds(userIds));
        }

        public Task<bool> DoesMemberExists(string userId)
        {
            return DoesUserExists<MemberDto>("member/find/", userId);
        }

        public async Task<Result<IEnumerable<TrainerDto>>> GetAllTrainersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || !userIds.Any())
            {
                return new Result<IEnumerable<TrainerDto>>(Array.Empty<TrainerDto>());
            }

            return await SendGetRequestAsync<IEnumerable<TrainerDto>>("trainer/by-ids/" + JoinUserIds(userIds));
        }

        public Task<bool> DoesTrainerExists(string userId)
        {
            return DoesUserExists<TrainerDto>("trainer/find/", userId);
        }

        public async Task<Result<IEnumerable<WorkerDto>>> GetAllWorkersWithIds(IEnumerable<string> userIds)
        {
            if (userIds is null || !userIds.Any())
            {
                return new Result<IEnumerable<WorkerDto>>(Array.Empty<WorkerDto>());
            }

            return await SendGetRequestAsync<IEnumerable<WorkerDto>>("worker/by-ids/" + JoinUserIds(userIds));
        }

        public Task<bool> DoesWorkerExists(string userId)
        {
            return DoesUserExists<WorkerDto>("worker/find/", userId);
        }

        public Task<bool> DoesOwnerExists(string userId)
        {
            return DoesUserExists<OwnerDto>("owner/find/", userId);
        }

        public async Task<bool> DoesUserExists<TUserDto>(string methodPath, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var path = methodPath + userId;

            var userData = await SendGetRequestAsync<TUserDto>(path);

            if (!userData.IsSuccess)
            {
                if (userData.Errors.Contains(Common.CommonConsts.NOT_FOUND))
                {
                    return false;
                }
                throw new InvalidInputException(userData.ErrorCombined);
            }

            return userData.Value != null;
        }

        private string JoinUserIds(IEnumerable<string> userIds)
        {
            return string.Join(Separator, userIds);
        }
    }
}
