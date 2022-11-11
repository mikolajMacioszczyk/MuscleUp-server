using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;
namespace Carnets.Application.Entries.Queries
{
    public record GetEnteredMembersEntriesQuery(string MemberId, int PageNumber, int PageSize) : IRequest<IEnumerable<Entry>>
    { }

    public class GetEnteredMembersEntriesQueryHandler : IRequestHandler<GetEnteredMembersEntriesQuery, IEnumerable<Entry>>
    {
        private readonly IEntryRepository _entryRepository;

        public GetEnteredMembersEntriesQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<IEnumerable<Entry>> Handle(GetEnteredMembersEntriesQuery request, CancellationToken cancellationToken)
        {
            var entries = await _entryRepository.GetGympassEntries(
                g => g.Gympass.UserId == request.MemberId, 
                request.PageNumber, 
                request.PageSize, 
                asTracking: false);

            return entries.Where(e => e.Entered);
        }
    }
}
