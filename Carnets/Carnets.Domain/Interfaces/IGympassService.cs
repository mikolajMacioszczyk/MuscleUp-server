using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface IGympassService
    {
        Task<IEnumerable<Gympass>> GetAll();

        Task<IEnumerable<Gympass>> GetAllFromFitnessClub(string fitnessClubId);

        Task<Gympass> GetById(string gympassId);

        Task<Gympass> GetByIdAndFitnessClub(string gympassId, string fitnessClubId);

        Task<Gympass> GetByIdAndMeber(string gympassId, string memberId);

        Task<Result<Gympass>> CreateGympass(string userId, string gympassTypeId);

        Task<Result<Subscription>> CreateGympassSubscription(string userId, string gympassId, Subscription subscription);

        Task<Result<Subscription>> CreateGympassSubscriptionWithFitnessClub(string fitnessClubId, string gympassId, Subscription subscription);

        Task<Result<Gympass>> CancelGympass(string gympassId, string memberId);
        
        Task<Result<Gympass>> CancelGympassByFitnessClub(string gympassId, string fitnessClubId);

        Task<Result<Gympass>> ActivateGympass(string gympassId, string memberId);

        Task<Result<Gympass>> ActivateGympassByFitnessClub(string gympassId, string fitnessClubId);

        Task<Result<Gympass>> DeactivateGympass(string gympassId, string memberId);

        Task<Result<Gympass>> DeactivateGympassyByFitnessClub(string gympassId, string fitnessClubId);

    }
}
