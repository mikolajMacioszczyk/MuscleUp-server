using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessClubs.Repo.Repositories
{
    public class UserInvitationRepository : IUserInvitationRepository
    {
        private readonly FitnessClubsDbContext _context;

        public UserInvitationRepository(FitnessClubsDbContext context)
        {
            _context = context;
        }

        public Task<UserInvitation> GetUserInvitationById(string invitationId, bool asTracking)
        {
            IQueryable<UserInvitation> query = _context.UserInvitations
                .Include(i => i.FitnessClub);

            if (!asTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefaultAsync(f => f.InvitationId == invitationId);
        }

        public async Task<UserInvitation> CreateUserInvitation(UserInvitation invitation)
        {
            if (invitation is null) throw new ArgumentException();

            invitation.InvitationId = Guid.NewGuid().ToString();

            await _context.UserInvitations.AddAsync(invitation);

            return invitation;
        }

        public async Task<Result<UserInvitation>> UpdateUserInvitation(string invitationId, UserInvitation invitation)
        {
            var invitationFromDb = await GetUserInvitationById(invitationId, true);

            if (invitationFromDb is null)
            {
                return new Result<UserInvitation>(Common.CommonConsts.NOT_FOUND);
            }

            invitationFromDb.FitnessClub = invitation.FitnessClub;
            invitationFromDb.UserType = invitation.UserType;
            invitationFromDb.Email = invitation.Email;
            invitationFromDb.ExpirationDateTime = invitation.ExpirationDateTime;
            invitationFromDb.IsUsed = invitation.IsUsed;

            return new Result<UserInvitation>(invitationFromDb);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
