﻿using Carnets.Domain.Enums;
using Common.Attribute;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.GympassTypes.Dtos
{
    public class CreateGympassTypeDto
    {
        [Required]
        [MaxLength(100)]
        public string GympassTypeName { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [MaxLength(10_000)]
        public string Description { get; set; }

        [Range(0, 1440)]
        [LessThan(nameof(EnableEntryToInMinutes), ErrorMessage = $"{nameof(EnableEntryFromInMinutes)} must be less then {nameof(EnableEntryToInMinutes)}")]
        public int EnableEntryFromInMinutes { get; set; }

        [Range(0, 1440)]
        public int EnableEntryToInMinutes { get; set; }

        public IntervalType Interval { get; set; }

        [Range(1, int.MaxValue)]
        public int IntervalCount { get; set; }

        [Range(0, int.MaxValue)]
        public int AllowedEntries { get; set; }

        public GympassTypeValidation ValidationType { get; set; }

        public IEnumerable<string> ClassPermissions { get; set; }

        public IEnumerable<string> PerkPermissions { get; set; }
    }
}
