using Carnets.Application.Entries.Dtos;
using Carnets.Application.Entries.Helpers;
using Carnets.Application.Interfaces;
using Common;
using Common.Models;
using MediatR;

namespace Carnets.Application.Entries.Queries
{
    public record GenerateEntryTokenQuery(string GympassId) : IRequest<Result<EntryTokenDto>>
    {
    }

    public class GenerateEntryTokenQueryHandler : IRequestHandler<GenerateEntryTokenQuery, Result<EntryTokenDto>>
    {
        private readonly IEntryTokenService _entryTokenService;
        private readonly IGympassRepository _gympassRepository;

        public GenerateEntryTokenQueryHandler(
            IEntryTokenService entryTokenService, 
            IGympassRepository gympassRepository)
        {
            _entryTokenService = entryTokenService;
            _gympassRepository = gympassRepository;
        }

        public async Task<Result<EntryTokenDto>> Handle(GenerateEntryTokenQuery request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, false);

            if (gympass is null)
            {
                return new Result<EntryTokenDto>(CommonConsts.NOT_FOUND);
            }

            var gympassValidationResult = EntryHelper.CanGympassEnterGym(gympass);

            if (!gympassValidationResult.result)
            {
                return new Result<EntryTokenDto>(gympassValidationResult.reason);
            }

            var token = new EntryTokenDto()
            {
                EntryToken = _entryTokenService.GenerateToken(request.GympassId)
            };

            return new Result<EntryTokenDto>(token);
        }
    }
}
