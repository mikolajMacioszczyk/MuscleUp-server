using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models
{
    public class FitnessClub
    {
        [Key]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string FitnessClubName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(500)]
        public string FitnessClubLogoUrl { get; set; }

        [Required]
        [MaxLength(36)]
        public string OwnerId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
