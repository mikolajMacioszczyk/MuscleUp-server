using Carnets.Application.Consts;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Carnets.Application.Entries.Commands
{
    public record GenerateEntryTokenCommand(string GympassId) : IRequest<Result<GeneratedEndtryTokenDto>>
    {
    }

    public class GenerateEntryTokenCommandHandler : IRequestHandler<GenerateEntryTokenCommand, Result<GeneratedEndtryTokenDto>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly int _validityInSeconds;

        public GenerateEntryTokenCommandHandler(
            IGympassRepository gympassRepository,
            IConfiguration configuration,
            IEntryRepository entryRepository)
        {
            _gympassRepository = gympassRepository;
            _entryRepository = entryRepository;
            _validityInSeconds = configuration.GetValue<int>(CarnetsConsts.EntryTokenValidityInSecondsKey);
        }

        public async Task<Result<GeneratedEndtryTokenDto>> Handle(GenerateEntryTokenCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<GeneratedEndtryTokenDto>(CommonConsts.NOT_FOUND);
            }

            var gympassValidationResult = EntryHelper.CanGympassEnterGym(gympass);

            if (!gympassValidationResult.result)
            {
                return new Result<GeneratedEndtryTokenDto>(gympassValidationResult.reason);
            }

            var entry = await CreateEntry(gympass);

            var token = new GeneratedEndtryTokenDto()
            {
                EntryToken = entry.EntryId,
                ValidityInSeconds = _validityInSeconds
            };

            return new Result<GeneratedEndtryTokenDto>(token);
        }

        private async Task<Entry> CreateEntry(Gympass gympass)
        {
            var entry = new Entry()
            {
                Gympass = gympass,
                CheckInTime = DateTime.UtcNow,
                EntryExpirationTime = DateTime.UtcNow.AddSeconds(_validityInSeconds),
                Entered = false
            };

            entry = await _entryRepository.CreateEntry(entry);
            await _entryRepository.SaveChangesAsync();

            return entry;
        }
    }
}
