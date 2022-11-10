using Common.Enums;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.UserInvitations.Dtos
{
    public class UserInvitationDto
    {
        public string InvitationId { get; set; }

        public RoleType UserType { get; set; }

        public FitnessClub FitnessClub { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public string Email { get; set; }
    }
}
