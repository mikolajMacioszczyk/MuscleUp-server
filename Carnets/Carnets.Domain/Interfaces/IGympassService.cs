using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
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

        Task<Result<(Gympass gympass, string checkoutSessionUrl)>> CreateGympass(string userId, CreateGympassDto model);

        Task<Result<Subscription>> CreateGympassSubscription(string userId, string gympassId, Subscription subscription);

        Task<Result<Subscription>> CreateGympassSubscriptionWithFitnessClub(string fitnessClubId, string gympassId, Subscription subscription);

        Task<Result<Gympass>> CancelGympass(string gympassId, string memberId);
        
        Task<Result<Gympass>> CancelGympassByFitnessClub(string gympassId, string fitnessClubId);

        Task<Result<Gympass>> ActivateGympass(string gympassId);

        Task<Result<Gympass>> ActivateGympassByFitnessClub(string gympassId, string fitnessClubId);

        Task<Result<Gympass>> DeactivateGympass(string gympassId);

        Task<Result<Gympass>> DeactivateGympassyByFitnessClub(string gympassId, string fitnessClubId);

        Task<Result<Gympass>> UpdateGympassEntries(string gympassId, int entries);

        Task<Result<Gympass>> ReduceGympassEntries(string gympassId);
    }
}
