using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Entries.Queries
{
    public record GetEntryByIdQuery(string EntryId) : IRequest<Entry>
    {
    }

    public class GetEntryByIdQueryHandler : IRequestHandler<GetEntryByIdQuery, Entry>
    {
        private readonly IEntryRepository _entryRepository;

        public GetEntryByIdQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public Task<Entry> Handle(GetEntryByIdQuery request, CancellationToken cancellationToken)
        {
            return _entryRepository.GetEntryById(request.EntryId, false);
        }
    }
}
