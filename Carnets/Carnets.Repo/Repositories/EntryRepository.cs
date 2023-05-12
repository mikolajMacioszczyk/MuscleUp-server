using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                .ThenInclude(g => g.GympassType)
                .Where(e => e.EntryId == entryId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entry>> SearchEntries(
            Expression<Func<Entry, bool>> predicate, 
            int pageNumber, 
            int pageSize, 
            bool asTracking)
        {
            var query = _context.Entries
                .Include(e => e.Gympass)
                .ThenInclude(e => e.GympassType)
                .Where(predicate)
                .Skip(pageNumber * pageSize)
                .Take(pageSize);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Entry> CreateEntry(Entry entry)
        {
            if (entry == null) throw new ArgumentException(nameof(entry));

            entry.EntryId = Guid.NewGuid().ToString();

            await _context.Entries.AddAsync(entry);

            return entry;
        }

        public async Task<Result<Entry>> UpdateEntry(string entryId, Entry entry)
        {
            if (entry is null) throw new ArgumentException(nameof(entry));

            var entryFromDb = await GetEntryById(entryId, true);

            if (entryFromDb is null)
            {
                return new Result<Entry>(Common.CommonConsts.NOT_FOUND);
            }

            entryFromDb.CheckInTime = entry.CheckInTime;
            entryFromDb.CheckOutTime = entry.CheckOutTime; 
            entryFromDb.Entered = entry.Entered;
            entryFromDb.EntryExpirationTime = entry.EntryExpirationTime;

            return new Result<Entry>(entryFromDb);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
