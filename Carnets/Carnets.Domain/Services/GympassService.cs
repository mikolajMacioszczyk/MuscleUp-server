using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class GympassService : IGympassService
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionService _subscriptionService;

        public GympassService(IGympassRepository gympassRepository, ISubscriptionService subscriptionService)
        {
            _gympassRepository = gympassRepository;
            _subscriptionService = subscriptionService;
        }

        public Task<IEnumerable<Gympass>> GetAll() => _gympassRepository.GetAll(false);

        public Task<IEnumerable<Gympass>> GetAllFromFitnessClub(string fitnessClubId) => 
            _gympassRepository.GetAllFromFitnessClub(fitnessClubId, false);

        public Task<Gympass> GetById(string gympassId) => 
            _gympassRepository.GetById(gympassId, false);

        public Task<Gympass> GetByIdAndFitnessClub(string gympassId, string fitnessClubId) =>
            GetByIdAndFitnessClub(gympassId, fitnessClubId, false);

        private async Task<Gympass> GetByIdAndFitnessClub(string gympassId, string fitnessClubId, bool asTracking)
        {
            var gympass = await _gympassRepository.GetById(gympassId, asTracking);

            return gympass.GympassType.FitnessClubId == fitnessClubId ? gympass : null;
        }

        public Task<Gympass> GetByIdAndMeber(string gympassId, string memberId) =>
            GetByIdAndMeber(gympassId, memberId, false);

        private async Task<Gympass> GetByIdAndMeber(string gympassId, string memberId, bool asTracking)
        {
            var gympass = await _gympassRepository.GetById(gympassId, false);

            return gympass.UserId == memberId ? gympass : null;
        }

        public async Task<Result<Gympass>> CreateGympass(string userId, string gympassTypeId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(nameof(userId));
            }

            var created = new Gympass
            {
                GympassId = Guid.NewGuid().ToString(),
                UserId = userId,
                Status = Enums.GympassStatus.New,
                ActivationDate = DateTime.MinValue,
                ValidityDate = DateTime.MinValue,
            };

            var createResult = await _gympassRepository.CreateGympass(gympassTypeId, created);

            if (createResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return createResult;
        }

        public async Task<Result<Subscription>> CreateGympassSubscription(string userId, string gympassId, Subscription subscription)
        {
            var gympass = await GetByIdAndMeber(gympassId, userId, true);

            return await CreateGympassSubscriptionHelper(gympass, subscription);
        }

        public async Task<Result<Subscription>> CreateGympassSubscriptionWithFitnessClub(string fitnessClubId, string gympassId, Subscription subscription)
        {
            var gympass = await GetByIdAndFitnessClub(gympassId, fitnessClubId, true);

            return await CreateGympassSubscriptionHelper(gympass, subscription);
        }

        public async Task<Result<Subscription>> CreateGympassSubscriptionHelper(Gympass gympass, Subscription subscription)
        {
            if (gympass is null)
            {
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != Enums.GympassStatus.New)
            {
                return new Result<Subscription>($"Cannot create subscription for gympass with status {gympass.Status}");
            }

            subscription.Gympass = gympass;
            subscription.SubscriptionId = Guid.NewGuid().ToString();

            gympass.Status = Enums.GympassStatus.Paid;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);
            if (!updateResult.IsSuccess)
            {
                return new Result<Subscription>(updateResult.Errors);
            }

            var createSubscriptionsResult = await _subscriptionService.CreateSubscription(subscription);

            if (createSubscriptionsResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return createSubscriptionsResult;
        }

        public async Task<Result<Gympass>> ActivateGympassByFitnessClub(string gympassId, string fitnessClubId)
        {
            var gympass = await GetByIdAndFitnessClub(gympassId, fitnessClubId, true);

            return await ActivateGympassHelper(gympass);
        }

        public async Task<Result<Gympass>> ActivateGympass(string gympassId, string memberId)
        {
            var gympass = await GetByIdAndMeber(gympassId, memberId, true);

            return await ActivateGympassHelper(gympass);
        }

        private async Task<Result<Gympass>> ActivateGympassHelper(Gympass gympass)
        {
            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != Enums.GympassStatus.Paid && gympass.Status != Enums.GympassStatus.Inactive)
            {
                return new Result<Gympass>($"Cannot activate gympass in status: {gympass.Status}");
            }
            
            if (gympass.RemainingValidityPeriodInSeconds <= 0)
            {
                return new Result<Gympass>($"The Gympass validity has ended");
            }

            var now = DateTime.UtcNow;
            if (gympass.Status == Enums.GympassStatus.Paid)
            {
                gympass.ActivationDate = now;
            }
            gympass.ValidityDate = now.AddSeconds(gympass.RemainingValidityPeriodInSeconds);

            gympass.Status = Enums.GympassStatus.Active;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }

        public async Task<Result<Gympass>> CancelGympass(string gympassId, string memberId)
        {
            var gympass = await GetByIdAndMeber(gympassId, memberId, true);

            return await CancelGympassHelper(gympass);
        }

        public async Task<Result<Gympass>> CancelGympassByFitnessClub(string gympassId, string fitnessClubId)
        {
            var gympass = await GetByIdAndFitnessClub(gympassId, fitnessClubId, true);

            return await CancelGympassHelper(gympass);
        }

        private async Task<Result<Gympass>> CancelGympassHelper(Gympass gympass)
        {
            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != Enums.GympassStatus.New)
            {
                return new Result<Gympass>($"Cannot cancell gympass in status: {gympass.Status}");
            }

            gympass.Status = Enums.GympassStatus.Cancelled;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }

        public async Task<Result<Gympass>> DeactivateGympass(string gympassId, string memberId)
        {
            var gympass = await GetByIdAndMeber(gympassId, memberId, true);

            return await DeactivateGympassHelper(gympass);
        }

        public async Task<Result<Gympass>> DeactivateGympassyByFitnessClub(string gympassId, string fitnessClubId)
        {
            var gympass = await GetByIdAndFitnessClub(gympassId, fitnessClubId, true);

            return await DeactivateGympassHelper(gympass);
        }

        private async Task<Result<Gympass>> DeactivateGympassHelper(Gympass gympass)
        {
            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != Enums.GympassStatus.Active)
            {
                return new Result<Gympass>($"Cannot deactivate gympass in status: {gympass.Status}");
            }

            gympass.Status = Enums.GympassStatus.Inactive;
            gympass.RemainingValidityPeriodInSeconds = (int) gympass.ValidityDate.Subtract(DateTime.UtcNow).TotalSeconds;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
