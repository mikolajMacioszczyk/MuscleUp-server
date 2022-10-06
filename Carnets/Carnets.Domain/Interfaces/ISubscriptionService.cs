using Carnets.Domain.Models;
using Common.Models;

namespace Carnets.Domain.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetSubscriptionById(string subscriptionId);

        Task<IEnumerable<Subscription>> GetAllGympassSubscriptions(string gympassId, string memberId);

        Task<IEnumerable<Subscription>> GetAllMemberSubscriptions(string memberId);

        Task<Result<Subscription>> CreateSubscription(Subscription subscription);
    }
}
