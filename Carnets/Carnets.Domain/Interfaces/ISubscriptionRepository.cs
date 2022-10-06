using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetSubscriptionById(string subcriptionId, bool asTracking);

        Task<IEnumerable<Subscription>> GetAllGympassSubscriptions(IEnumerable<string> gympassIds, bool asTracking);

        Task<IEnumerable<Subscription>> GetAllCustomerSubscriptions(string stripeCustomerId, bool asTracking);

        Task<Result<Subscription>> CreateSubscription(Subscription subscription);

        Task SaveChangesAsync();
    }
}
