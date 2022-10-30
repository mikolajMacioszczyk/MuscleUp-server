using Carnets.Application.Consts;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Helpers;
using Carnets.Application.Interfaces;
using Common;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Carnets.Application.Entries.Queries
{
    public record GenerateEntryTokenQuery(string GympassId) : IRequest<Result<GeneratedEndtryTokenDto>>
    {
    }

    public class GenerateEntryTokenQueryHandler : IRequestHandler<GenerateEntryTokenQuery, Result<GeneratedEndtryTokenDto>>
    {
        private readonly IEntryTokenService _entryTokenService;
        private readonly IGympassRepository _gympassRepository;
        private readonly int _validityInSeconds;

        public GenerateEntryTokenQueryHandler(
            IEntryTokenService entryTokenService, 
            IGympassRepository gympassRepository,
            IConfiguration configuration)
        {
            _entryTokenService = entryTokenService;
            _gympassRepository = gympassRepository;
            _validityInSeconds = configuration.GetValue<int>(CarnetsConsts.EntryTokenValidityInSecondsKey);
        }

        public async Task<Result<GeneratedEndtryTokenDto>> Handle(GenerateEntryTokenQuery request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, false);

            if (gympass is null)
            {
                return new Result<GeneratedEndtryTokenDto>(CommonConsts.NOT_FOUND);
            }

            var gympassValidationResult = EntryHelper.CanGympassEnterGym(gympass);

            if (!gympassValidationResult.result)
            {
                return new Result<GeneratedEndtryTokenDto>(gympassValidationResult.reason);
            }

            var token = new GeneratedEndtryTokenDto()
            {
                EntryToken = _entryTokenService.GenerateToken(request.GympassId),
                ValidityInSeconds = _validityInSeconds
            };

            return new Result<GeneratedEndtryTokenDto>(token);
        }
    }
}
