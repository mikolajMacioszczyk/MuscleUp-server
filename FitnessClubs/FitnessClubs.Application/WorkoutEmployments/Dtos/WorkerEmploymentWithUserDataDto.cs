using Common.Models.Dtos;

namespace FitnessClubs.Application.WorkoutEmployments.Dtos
{
    public class WorkerEmploymentWithUserDataDto : WorkerEmploymentDto
    {
        public WorkerDto UserData { get; set; }
    }
}
