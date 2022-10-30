using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly CarnetsDbContext _context;

        public EntryRepository(CarnetsDbContext context)
        {
            _context = context;
        }

        public Task<Entry> GetEntryById(string entryId, bool asTracking)
        {
            var query = _context.Entries
                .Include(e => e.Gympass)
                .Where(e => e.EntryId == entryId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefaultAsync();
        }

        public async Task<Entry> CreateEntry(Entry entry)
        {
            if (entry == null) throw new ArgumentException(nameof(entry));

            entry.EntryId = Guid.NewGuid().ToString();

            await _context.Entries.AddAsync(entry);

            return entry;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
