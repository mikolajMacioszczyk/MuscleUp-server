using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models
{
    public class UserInvitation
    {
        [Key]
        [MaxLength(36)]
        public string InvitationId { get; set; }

        public RoleType UserType { get; set; }

        [Required]
        [MaxLength(36)]
        public FitnessClub FitnessClub { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public bool IsUsed { get; set; }
    }
}
