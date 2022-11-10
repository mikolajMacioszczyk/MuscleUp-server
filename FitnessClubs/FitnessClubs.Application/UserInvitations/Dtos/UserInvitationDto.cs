using Common.Enums;

namespace FitnessClubs.Application.UserInvitations.Dtos
{
    public class UserInvitationDto
    {
        public string InvitationId { get; set; }

        public RoleType UserType { get; set; }

        public string FitnessClubId { get; set; }

        public string FitnessClubName { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public string Email { get; set; }
    }
}
