using Carnets.Domain.Enums;

namespace Carnets.Domain.Models.Dtos
{
    public class GympassDto
    {
        public string GympassId { get; set; }

        public string UserId { get; set; }

        public string GympassTypeName { get; set; }

        public string GympassTypeId { get; set; }

        public DateTime ValidityDate { get; set; }

        public DateTime ActivationDate { get; set; }

        public GympassStatus Status { get; set; }

        public GympassTypeValidation ValidationType { get; set; }

        public int RemainingValidityPeriodInSeconds { get; set; }

        public int RemainingEntries { get; set; }
    }
}
