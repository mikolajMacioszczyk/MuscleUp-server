using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Entries.Queries
{
    public record GetEnteredGympassEntriesQuery(string GympassId, int PageNumber, int PageSize) : IRequest<IEnumerable<Entry>>
    {}

    public class GetEnteredGympassEntriesQueryHandler : IRequestHandler<GetEnteredGympassEntriesQuery, IEnumerable<Entry>>
    {
        private readonly IEntryRepository _entryRepository;

        public GetEnteredGympassEntriesQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<IEnumerable<Entry>> Handle(GetEnteredGympassEntriesQuery request, CancellationToken cancellationToken)
        {
            var entries = await _entryRepository.GetGympassEntries(
                g => g.Gympass.GympassId == request.GympassId, 
                request.PageNumber, 
                request.PageSize, 
                asTracking: false);

            return entries.Where(e => e.Entered);
        }
    }
}
