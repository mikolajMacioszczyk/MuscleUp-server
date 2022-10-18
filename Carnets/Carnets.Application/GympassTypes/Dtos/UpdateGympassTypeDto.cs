using Carnets.Domain.Enums;
using Common.Attribute;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.GympassTypes.Dtos
{
    public class UpdateGympassTypeDto
    {
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [MaxLength(10_000)]
        public string Description { get; set; }

        [Range(0, 1440)]
        [LessThan(nameof(EnableEntryToInMinutes), ErrorMessage = $"{nameof(EnableEntryFromInMinutes)} must be less then {nameof(EnableEntryToInMinutes)}")]
        public int EnableEntryFromInMinutes { get; set; }

        [Range(0, 1440)]
        public int EnableEntryToInMinutes { get; set; }

        [Range(0, int.MaxValue)]
        public int ValidityPeriodInSeconds { get; set; }

        [Range(0, int.MaxValue)]
        public int AllowedEntries { get; set; }

        public GympassTypeValidation ValidationType { get; set; }
    }
}
