using Carnets.Domain.Models;

namespace Carnets.Application.Interfaces
{
    public interface IEntryRepository
    {
        Task<Entry> GetEntryById(string entryId, bool asTracking);

        Task<Entry> CreateEntry(Entry entry);

        Task SaveChangesAsync();
    }
}
