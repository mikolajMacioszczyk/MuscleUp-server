using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Application.Interfaces
{
    public interface IEntryRepository
    {
        Task<Entry> GetEntryById(string entryId, bool asTracking);

        Task<IEnumerable<Entry>> GetGympassEntries(string gympassId, int pageNumber, int pageSize, bool asTracking);

        Task<Entry> CreateEntry(Entry entry);

        Task<Result<Entry>> UpdateEntry(string entryId, Entry entry);

        Task SaveChangesAsync();
    }
}
