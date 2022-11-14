using Carnets.Domain.Models;
using Common.Models;
using System.Linq.Expressions;

namespace Carnets.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetSubscriptionById(string subcriptionId, bool asTracking);

        Task<Subscription> GetSubscription(Expression<Func<Subscription, bool>> predicate, bool asTracking);

        Task<IEnumerable<Subscription>> GetAllGympassSubscriptions(IEnumerable<string> gympassIds, bool asTracking);

        Task<IEnumerable<Subscription>> GetAllCustomerSubscriptions(string stripeCustomerId, bool asTracking);

        Task<Result<Subscription>> CreateSubscription(Subscription subscription);

        Task<Result<Subscription>> UpdateSubscription(string subscriptionId, Subscription subscription);

        Task SaveChangesAsync();
    }
}
