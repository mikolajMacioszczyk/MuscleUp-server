using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.Memberships.Dtos
{
    public class CreateMembershipDto
    {
        [Required]
        [MaxLength(36)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }
    }
}
