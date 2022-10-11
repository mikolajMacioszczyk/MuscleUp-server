using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.WorkoutEmployments.Dtos
{
    public class WorkerEmploymentDto
    {
        public string WorkerEmploymentId { get; set; }

        public string UserId { get; set; }

        public FitnessClub FitnessClub { get; set; }

        public DateTime EmployedFrom { get; set; }

        public DateTime? EmployedTo { get; set; }

        public bool IsActive => !EmployedTo.HasValue;
    }
}
