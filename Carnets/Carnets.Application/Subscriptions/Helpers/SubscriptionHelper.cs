using Carnets.Application.Interfaces;
using Carnets.Domain.Models;

namespace Carnets.Application.Subscriptions.Helpers
{
    public static class SubscriptionHelper
    {
        public static async Task<Subscription> GetActiveGympassSubscriptionByExternalId(
            string gympassId, 
            string externalSubscriptionId, 
            ISubscriptionRepository subscriptionRepository)
        {
            var gympassSubscriptions = await subscriptionRepository.GetAllGympassSubscriptions(new string[] { gympassId }, true);

            return gympassSubscriptions.FirstOrDefault(g => g.ExternalSubscriptionId == externalSubscriptionId && g.IsActive);
        }
    }
}
