using Common.Models.Dtos;

namespace FitnessClubs.Application.TrainerEmployments.Dtos
{
    public class TrainerEmploymentWithUserDataDto : TrainerEmploymentDto
    {
        public TrainerDto TrainerData { get; set; }
    }
}
