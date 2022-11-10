using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.UserInvitations.Dtos
{
    public class ConsumeUserInvitationDto
    {
        [Required]
        [MaxLength(36)]
        public string InvitationId { get; set; }
    }
}
