using Common.Attribute;
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

        [Range(0, 1440)]
        [LessThan(nameof(EnableEntryToInMinutes), ErrorMessage = $"{nameof(EnableEntryFromInMinutes)} must be less then {nameof(EnableEntryToInMinutes)}")]
        public int EnableEntryFromInMinutes { get; set; }

        [Range(0, 1440)]
        public int EnableEntryToInMinutes { get; set; }

        [Range(0, int.MaxValue)]
        public int ValidityPeriodInSeconds { get; set; }
    }
}
