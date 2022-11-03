using Common.Models.Dtos;

namespace FitnessClubs.Application.TrainerEmployments.Dtos
{
    public class TrainerEmploymentWithUserDataDto : TrainerEmploymentDto
    {
        public TrainerDto UserData { get; set; }
    }
}
