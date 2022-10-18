using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Subscriptions.Queries
{
    public record GetSubscriptionByIdQuery : IRequest<Subscription>
    {
        public string SubscriptionId { get; init; }
    }

    public class GetSubscriptionByIdQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, Subscription>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetSubscriptionByIdQueryHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public Task<Subscription> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            return _subscriptionRepository.GetSubscriptionById(request.SubscriptionId, false);
        }
    }
}
