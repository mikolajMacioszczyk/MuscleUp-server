using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.WorkoutEmployments.Dtos
{
    public class CreateWorkerEmploymentDto
    {
        [MaxLength(36)]
        public string UserId { get; set; }

        [MaxLength(36)]
        public string FitnessClubId { get; set; }
    }
}
