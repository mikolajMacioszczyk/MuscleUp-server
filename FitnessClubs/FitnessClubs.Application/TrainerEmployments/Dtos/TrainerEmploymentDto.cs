using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.TrainerEmployments.Dtos
{
    public class TrainerEmploymentDto
    {
        public string EmploymentId { get; set; }

        public string UserId { get; set; }

        public FitnessClub FitnessClub { get; set; }

        public DateTime EmployedFrom { get; set; }

        public DateTime? EmployedTo { get; set; }

        public bool IsActive => !EmployedTo.HasValue;
    }
}
