namespace Carnets.Domain.Models.Dtos
{
    public class GympassTypeDto
    {
        public string GympassTypeId { get; set; }

        public string GympassTypeName { get; set; }

        public double Price { get; set; }

        public int EnableEntryFromInMinutes { get; set; }

        public int EnableEntryToInMinutes { get; set; }

        public int ValidityPeriodInSeconds { get; set; }

        public bool IsActive { get; set; }

        public int Version { get; set; }
    }
}
