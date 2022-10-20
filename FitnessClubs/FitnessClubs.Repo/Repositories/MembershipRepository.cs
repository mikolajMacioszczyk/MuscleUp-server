using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly FitnessClubsDbContext _context;

        public MembershipRepository(FitnessClubsDbContext context)
        {
            _context = context;
        }

        public Task<Membership> GetMembershipById(string memberId, string fitnessClubId, bool asTracking)
        {
            var query = _context.Memberships
                .Where(m => m.FitnessClubId == fitnessClubId && m.MemberId == memberId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Membership>> GetAllMembershipsFromFitnessClub(string fitnessClubId, bool asTracking)
        {
            var query = _context.Memberships
                .Where(m => m.FitnessClubId == fitnessClubId);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<Membership> CreateMembership(Membership membership)
        {
            membership.JoiningDate = DateTime.UtcNow;
            
            await _context.Memberships.AddAsync(membership);

            return membership;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
