using Common.Models;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.Interfaces
{
    public interface IUserInvitationRepository
    {
        Task<UserInvitation> GetUserInvitationById(string invitationId, bool asTracking);

        Task<UserInvitation> CreateUserInvitation(UserInvitation invitation);

        Task<Result<UserInvitation>> UpdateUserInvitation(string invitationId, UserInvitation invitation);

        Task SaveChangesAsync();
    }
}
