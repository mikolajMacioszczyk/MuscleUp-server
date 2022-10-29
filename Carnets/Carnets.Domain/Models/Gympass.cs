using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Gympass
    {
        [Key]
        [MaxLength(36)]
        public string GympassId { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        public GympassType GympassType { get; set; }

        public DateTime ValidityDate { get; set; }

        public DateTime ActivationDate { get; set; }

        public GympassStatus Status { get; set; }

        [Range(0, int.MaxValue)]
        public int RemainingEntries { get; set; }
    }
}
