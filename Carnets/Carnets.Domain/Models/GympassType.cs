using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class GympassType
    {
        [Key]
        [MaxLength(30)]
        public string GympassTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string GympassTypeName { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        public int ValidityPeriodInSeconds { get; set; }
    }
}
