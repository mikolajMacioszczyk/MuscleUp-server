using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Gympass
    {
        [Key]
        [MaxLength(30)]
        public string GympassId { get; set; }

        [Required]
        [MaxLength(30)]
        public string UserId { get; set; }

        [Required]
        public GympassType GympassType { get; set; }

        [Required]
        [MaxLength(30)]
        public string FitnessClubId { get; set; }

        public DateTime ValidityDate { get; set; }

        public DateTime ActivationDate { get; set; }
    }
}
