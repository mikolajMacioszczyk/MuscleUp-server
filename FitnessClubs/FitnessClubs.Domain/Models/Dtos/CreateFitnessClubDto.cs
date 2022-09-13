using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models.Dtos
{
    public class CreateFitnessClubDto
    {
        [Required]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string FitnessClubName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }
    }
}
