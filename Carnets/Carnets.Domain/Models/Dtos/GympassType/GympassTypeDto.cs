using Carnets.Domain.Enums;

namespace Carnets.Domain.Models.Dtos
{
    public class GympassTypeDto
    {
        public string GympassTypeId { get; set; }

        public string GympassTypeName { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public int EnableEntryFromInMinutes { get; set; }

        public int EnableEntryToInMinutes { get; set; }

        public int ValidityPeriodInSeconds { get; set; }

        public int AllowedEntries { get; set; }

        public GympassTypeValidation ValidationType { get; set; }

        public bool IsActive { get; set; }

        public int Version { get; set; }

        public IEnumerable<string> ClassPermissions { get; set; }

        public IEnumerable<string> PerkPermissions { get; set; }
    }
}
