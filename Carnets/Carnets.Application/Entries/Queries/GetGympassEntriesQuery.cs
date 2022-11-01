using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Entries.Queries
{
    public record GetGympassEntriesQuery(string GympassId, int PageNumber, int PageSize) : IRequest<IEnumerable<Entry>>
    {}

    public class GetGympassEntriesQueryHandler : IRequestHandler<GetGympassEntriesQuery, IEnumerable<Entry>>
    {
        private readonly IEntryRepository _entryRepository;

        public GetGympassEntriesQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public Task<IEnumerable<Entry>> Handle(GetGympassEntriesQuery request, CancellationToken cancellationToken)
        {
            return _entryRepository.GetGympassEntries(request.GympassId, request.PageNumber, request.PageSize, false);
        }
    }
}
