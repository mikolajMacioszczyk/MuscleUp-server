using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Subscriptions.Queries
{
    public record GetAllMemberSubscriptionsQuery : IRequest<IEnumerable<Subscription>>
    {
        public string MemberId { get; init; }
    }

    public class GetAllMemberSubscriptionsQueryHandler : IRequestHandler<GetAllMemberSubscriptionsQuery, IEnumerable<Subscription>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IGympassRepository _gympassRepository;

        public GetAllMemberSubscriptionsQueryHandler(
            ISubscriptionRepository subscriptionRepository, 
            IGympassRepository gympassRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _gympassRepository = gympassRepository;
        }

        public async Task<IEnumerable<Subscription>> Handle(GetAllMemberSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var memberGympassess = await _gympassRepository.GetAllForMember(request.MemberId, false);
            var gympassesIds = memberGympassess.Select(s => s.GympassId);

            return await _subscriptionRepository.GetAllGympassSubscriptions(gympassesIds, false);
        }
    }
}
