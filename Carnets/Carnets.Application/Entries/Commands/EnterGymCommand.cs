using Carnets.Application.Entries.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.BaseClasses;
using Common.Exceptions;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Entries.Commands
{
    public record EnterGymCommand(EntryTokenDto EntryTokenDto, string FitnessClubId) : IRequest<Result<Entry>>
    { }

    public class EnterGymCommandHandler : ConcurrentTokenHandlerBase,
        IRequestHandler<EnterGymCommand, Result<Entry>>
    {
        private readonly ILogger<EnterGymCommandHandler> _logger;
        private readonly IGympassRepository _gympassRepository;
        private readonly IEntryRepository _entryRepository;

        public EnterGymCommandHandler(
            ILogger<EnterGymCommandHandler> logger,
            IGympassRepository gympassRepository,
            IEntryRepository entryRepository)
        {
            _gympassRepository = gympassRepository;
            _entryRepository = entryRepository;
            _logger = logger;
        }

        public async Task<Result<Entry>> Handle(EnterGymCommand request, CancellationToken cancellationToken)
        {
            while (!LockToken(request.EntryTokenDto.EntryToken))
            {
                Thread.Sleep(WaitMiliseconds);
            }

            try
            {
                var result = await HandleWithoutConcurrency(request, cancellationToken);
                ReleaseToken(request.EntryTokenDto.EntryToken);
                return result;
            }
            catch (Exception)
            {
                ReleaseToken(request.EntryTokenDto.EntryToken);
                throw;
            }
        }

        public async Task<Result<Entry>> HandleWithoutConcurrency(EnterGymCommand request, CancellationToken cancellationToken)
        {
            var entryResult = await ReadEntryToken(request.EntryTokenDto.EntryToken, request.FitnessClubId);
            if (!entryResult.IsSuccess)
            {
                return entryResult;
            }

            var entry = entryResult.Value;

            await ReduceGympassEntries(entry);
            await UpdateEntryData(entry);

            try
            {
                await _gympassRepository.SaveChangesAsync();
                await _entryRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }

            return new Result<Entry>(entry);
        }

        private async Task<Result<Entry>> ReadEntryToken(string token, string fitnessClubId)
        {
            var entry = await _entryRepository.GetEntryById(token, true);

            if (entry is null)
            {
                return new Result<Entry>("Invalid entry token");
            }

            if (entry.EntryExpirationTime < DateTime.UtcNow)
            {
                return new Result<Entry>("Entry token expired");
            }

            if (entry.Entered)
            {
                return new Result<Entry>("Entry token already used");
            }

            if (entry.Gympass.GympassType.FitnessClubId != fitnessClubId)
            {
                return new Result<Entry>($"Gympass does not belongs to fitness club");
            }

            return new Result<Entry>(entry);
        }

        private async Task ReduceGympassEntries(Entry entry)
        {
            var gympass = entry.Gympass;
            var reduceEntriesResult = gympass.ReduceEntries();

            if (!reduceEntriesResult.IsSuccess)
            {
                throw new InvalidInputException(reduceEntriesResult.ErrorCombined);
            }
            var updateResult = await _gympassRepository.UpdateGympass(gympass);
            if (!updateResult.IsSuccess)
            {
                throw new InvalidInputException(updateResult.ErrorCombined);
            }
        }

        private async Task UpdateEntryData(Entry entry)
        {
            entry.CheckInTime = DateTime.UtcNow;
            entry.Entered = true;

            var updateResult = await _entryRepository.UpdateEntry(entry.EntryId, entry);
            if (!updateResult.IsSuccess)
            {
                throw new InvalidInputException(updateResult.ErrorCombined);
            }
        }
    }
}
