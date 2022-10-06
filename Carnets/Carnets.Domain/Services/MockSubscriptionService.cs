using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Services
{
    public class MockSubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IGympassRepository _gympassRepository;

        public MockSubscriptionService(ISubscriptionRepository subscriptionRepository, IGympassRepository gympassRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _gympassRepository = gympassRepository;
        }

        public Task<Subscription> GetSubscriptionById(string subscriptionId) => 
            _subscriptionRepository.GetSubscriptionById(subscriptionId, false);

        public async Task<IEnumerable<Subscription>> GetAllGympassSubscriptions(string gympassId, string memberId)
        {
            var all = await _subscriptionRepository.GetAllGympassSubscriptions(new string[] { gympassId }, false);

            return all.Where(s => s.Gympass.UserId == memberId);
        }

        public async Task<IEnumerable<Subscription>> GetAllMemberSubscriptions(string memberId)
        {
            var memberGympassess = await _gympassRepository.GetAllForMember(memberId, false);
            var gympassesIds = memberGympassess.Select(s => s.GympassId);

            return await _subscriptionRepository.GetAllGympassSubscriptions(gympassesIds, false);
        }

        public async Task<Result<Subscription>> CreateSubscription(Subscription subscription)
        {
            // stripe logic

            var result = await _subscriptionRepository.CreateSubscription(subscription);
            
            if (result.IsSuccess)
            {
                await _subscriptionRepository.SaveChangesAsync();
            }

            return result;
        }
    }
}
