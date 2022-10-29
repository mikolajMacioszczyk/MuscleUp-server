using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Subscriptions.Queries
{
    public record GetAllGympassSubscriptionsQuery : IRequest<IEnumerable<Subscription>>
    {
        public string GympassId { get; init; }
    }

    public class GetAllGympassSubscriptionsQueryHandler : IRequestHandler<GetAllGympassSubscriptionsQuery, IEnumerable<Subscription>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetAllGympassSubscriptionsQueryHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IEnumerable<Subscription>> Handle(GetAllGympassSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            return await _subscriptionRepository.GetAllGympassSubscriptions(new string[] { request.GympassId }, false);
        }
    }
}
