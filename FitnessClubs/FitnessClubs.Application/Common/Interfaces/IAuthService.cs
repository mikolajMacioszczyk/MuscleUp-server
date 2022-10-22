using Common.Models;
using Common.Models.Dtos;

namespace FitnessClubs.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<IEnumerable<WorkerDto>>> GetAllWorkersWithIds(IEnumerable<string> userIds);
        Task<bool> DoesWorkerExists(string userId);

        Task<Result<IEnumerable<MemberDto>>> GetAllMembersWithIds(IEnumerable<string> userIds);
        Task<bool> DoesMemberExists(string userId);
        
        Task<Result<IEnumerable<TrainerDto>>> GetAllTrainersWithIds(IEnumerable<string> userIds);
        Task<bool> DoesTrainerExists(string userId);
    }
}
