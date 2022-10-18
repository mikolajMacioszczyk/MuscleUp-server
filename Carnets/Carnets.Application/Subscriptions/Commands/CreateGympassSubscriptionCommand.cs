using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CreateGympassSubscriptionCommand : IRequest<Result<Subscription>>
    {
        public string GympassId { get; init; }
        public Subscription Subscription { get; init; }
    }

    public class CreateGympassSubscriptionCommandHandler : IRequestHandler<CreateGympassSubscriptionCommand, Result<Subscription>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public CreateGympassSubscriptionCommandHandler(
            IGympassRepository gympassRepository, 
            ISubscriptionRepository subscriptionRepository)
        {
            _gympassRepository = gympassRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<Result<Subscription>> Handle(CreateGympassSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != GympassStatus.New)
            {
                return new Result<Subscription>($"Cannot create subscription for gympass with status {gympass.Status}");
            }

            request.Subscription.Gympass = gympass;
            request.Subscription.SubscriptionId = Guid.NewGuid().ToString();

            gympass.Status = GympassStatus.Active;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);
            if (!updateResult.IsSuccess)
            {
                return new Result<Subscription>(updateResult.Errors);
            }

            var createSubscriptionResult = await CreateSubscription(request.Subscription);

            if (createSubscriptionResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return createSubscriptionResult;
        }

        private async Task<Result<Subscription>> CreateSubscription(Subscription subscription)
        {
            var result = await _subscriptionRepository.CreateSubscription(subscription);

            if (result.IsSuccess)
            {
                await _subscriptionRepository.SaveChangesAsync();
            }

            return result;
        }
    }
}
