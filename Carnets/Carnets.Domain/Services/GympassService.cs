using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;
using Common.Enums;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class GympassService : IGympassService
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionService _subscriptionService;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IPaymentService _paymentService;

        public GympassService(
            IGympassRepository gympassRepository, 
            ISubscriptionService subscriptionService, 
            HttpAuthContext httpAuthContext, 
            IPaymentService paymentService)
        {
            _gympassRepository = gympassRepository;
            _subscriptionService = subscriptionService;
            _httpAuthContext = httpAuthContext;
            _paymentService = paymentService;
        }

        public Task<IEnumerable<Gympass>> GetAll()
        {
            if (_httpAuthContext.UserRole == RoleType.Member)
            {
                var memberId = _httpAuthContext.UserId;
                return _gympassRepository.GetAllForMember(memberId, false);
            }

            return _gympassRepository.GetAll(false);
        }

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

        public async Task<Result<(Gympass gympass, string checkoutSessionUrl)>> CreateGympass(string userId, CreateGympassDto model)
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

            var createResult = await _gympassRepository.CreateGympass(model.GympassTypeId, created);
            if (createResult.IsSuccess)
            {
                var gympassTypeId = createResult.Value.GympassType.GympassTypeId;
                var gympassId = createResult.Value.GympassId;
                var customerId = await _paymentService.GetOrCreateCustomer(userId);

                var checkoutSessionUrl = await _paymentService.CreateCheckoutSession(gympassId, customerId, 
                    gympassTypeId, model.SuccessUrl, model.CancelUrl);

                await _gympassRepository.SaveChangesAsync();

                return new Result<(Gympass, string)>((createResult.Value, checkoutSessionUrl));
            }
            return new Result<(Gympass, string)>(createResult.Errors);
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

            gympass.Status = Enums.GympassStatus.Active;

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

        public async Task<Result<Gympass>> ActivateGympass(string gympassId)
        {
            var gympass = await _gympassRepository.GetById(gympassId, true);

            return await ActivateGympassHelper(gympass);
        }

        private async Task<Result<Gympass>> ActivateGympassHelper(Gympass gympass)
        {
            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != Enums.GympassStatus.New && gympass.Status != Enums.GympassStatus.Inactive)
            {
                return new Result<Gympass>($"Cannot activate gympass in status: {gympass.Status}");
            }
            
            if (gympass.RemainingValidityPeriodInSeconds <= 0)
            {
                return new Result<Gympass>($"The Gympass validity has ended");
            }

            var now = DateTime.UtcNow;
            if (gympass.Status == Enums.GympassStatus.New)
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

        public async Task<Result<Gympass>> DeactivateGympass(string gympassId)
        {
            var gympass = await _gympassRepository.GetById(gympassId, true);

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

            if (gympass.Status != Enums.GympassStatus.Active || gympass.Status != Enums.GympassStatus.New)
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

        public Task<Result<Gympass>> UpdateGympassEntries(string gympassId, int entries) =>
            UpdateGympassEntriesHelper(gympassId, gympass => entries);

        public Task<Result<Gympass>> ReduceGympassEntries(string gympassId) =>
            UpdateGympassEntriesHelper(gympassId, gympass => gympass.RemainingEntries - 1);

        public async Task<Result<Gympass>> UpdateGympassEntriesHelper(string gympassId, Func<Gympass, int> updateEntries)
        {
            var gympass = await _gympassRepository.GetById(gympassId, true);

            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.GympassType.ValidationType != Enums.GympassTypeValidation.Entries)
            {
                return new Result<Gympass>($"Cannot update entries of gympass with validation type \"{gympass.GympassType.ValidationType}\"");
            }

            gympass.RemainingEntries = updateEntries(gympass);

            if (gympass.RemainingEntries < 0)
            {
                return new Result<Gympass>($"Remaining gympass entries cannot be less than 0");
            }

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
