using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Application.TrainerEmployments.Dtos
{
    public class CreateTrainerEmploymentDto
    {
        [MaxLength(36)]
        public string UserId { get; set; }

        [MaxLength(36)]
        public string FitnessClubId { get; set; }
    }
}
