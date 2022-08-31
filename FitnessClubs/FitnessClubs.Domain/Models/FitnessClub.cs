using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models
{
    public class FitnessClub
    {
        [Key]
        [MaxLength(30)]
        public string FitnessClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string FitnessClubName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }
    }
}
