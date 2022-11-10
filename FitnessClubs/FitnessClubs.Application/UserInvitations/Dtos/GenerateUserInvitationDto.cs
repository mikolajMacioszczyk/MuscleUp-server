using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.UserInvitations.Dtos
{
    public class GenerateUserInvitationDto
    {
        [Required]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string BaseInvitationLink { get; set; }
    }
}
