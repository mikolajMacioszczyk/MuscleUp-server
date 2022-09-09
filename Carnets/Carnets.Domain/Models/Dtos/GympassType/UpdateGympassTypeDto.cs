using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class UpdateGympassTypeDto
    {
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        public int ValidityPeriodInSeconds { get; set; }
    }
}
