using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models
{
    public class Membership
    {
        [Required]
        [MaxLength(36)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }

        public DateTime JoiningDate { get; set; }
    }
}
