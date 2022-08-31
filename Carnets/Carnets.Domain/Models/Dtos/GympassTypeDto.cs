namespace Carnets.Domain.Models.Dtos
{
    public class GympassTypeDto
    {
        public string GympassTypeId { get; set; }

        public string GympassTypeName { get; set; }

        public double Price { get; set; }

        public int ValidityPeriodInSeconds { get; set; }
    }
}
