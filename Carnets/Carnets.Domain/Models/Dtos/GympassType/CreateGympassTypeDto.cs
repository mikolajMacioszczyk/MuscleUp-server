using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateGympassTypeDto
    {
        [Required]
        [MaxLength(100)]
        public string GympassTypeName { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        public int ValidityPeriodInSeconds { get; set; }
    }
}
