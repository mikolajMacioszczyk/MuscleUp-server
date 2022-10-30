using AutoMapper;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common;
using Common.Exceptions;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Entries.Commands
{
    public record CreateGymEntryCommand(EntryTokenDto EntryTokenDto, string fitnessClubId) : IRequest<Result<Entry>>
    { }

    public class CreateGymEntryCommandHandler : IRequestHandler<CreateGymEntryCommand, Result<Entry>>
    {
        private readonly ILogger<CreateGymEntryCommandHandler> _logger;
        private readonly IEntryTokenService _entryTokenService;
        private readonly IGympassRepository _gympassRepository;
        private readonly IEntryRepository _entryRepository;

        public CreateGymEntryCommandHandler(
            ILogger<CreateGymEntryCommandHandler> logger,
            IEntryTokenService entryTokenService,
            IGympassRepository gympassRepository,
            IEntryRepository entryRepository,
            IMapper mapper)
        {
            _entryTokenService = entryTokenService;
            _gympassRepository = gympassRepository;
            _entryRepository = entryRepository;
            _logger = logger;
        }

        public async Task<Result<Entry>> Handle(CreateGymEntryCommand request, CancellationToken cancellationToken)
        {
            var decoded = _entryTokenService.DecodeToken(request.EntryTokenDto.EntryToken);

            if (decoded.ExpiresDate < DateTime.UtcNow)
            {
                return new Result<Entry>("Entry token expired");
            }

            var gympass = await _gympassRepository.GetById(decoded.GympassId, true);

            if (gympass is null)
            {
                throw new BadRequestException($"Gympass with id {decoded.GympassId} not found");
            }

            if (gympass.GympassType.FitnessClubId != request.fitnessClubId)
            {
                throw new BadRequestException($"Gympass does not belongs to fitness club");
            }

            var reduceEntriesResult = await EntryHelper.ReduceGympassEntries(gympass, _gympassRepository);

            if (!reduceEntriesResult.IsSuccess)
            {
                return new Result<Entry>(reduceEntriesResult.Errors);
            }

            var entry = await _entryRepository.CreateEntry(new Entry()
            {
                Gympass = gympass,
                CheckInTime = DateTime.UtcNow,
                CheckOutTime = null
            });

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
    }
}
