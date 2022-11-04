using Carnets.Application.Interfaces;
using Carnets.Application.Subscriptions.Helpers;
using Carnets.Domain.Models;
using Common;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record TerminateGympassSubscriptionCommand(string GympassId, string ExternalSubscriptionId) 
        : IRequest<Result<Subscription>>
    {}

    public class TerminateGympassSubscriptionCommandHandler : IRequestHandler<TerminateGympassSubscriptionCommand, Result<Subscription>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<CreateOrExtendGympassSubscriptionCommand> _logger;

        public TerminateGympassSubscriptionCommandHandler(
            IGympassRepository gympassRepository,
            ISubscriptionRepository subscriptionRepository,
            ILogger<CreateOrExtendGympassSubscriptionCommand> logger)
        {
            _gympassRepository = gympassRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<Result<Subscription>> Handle(TerminateGympassSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, false);

            if (gympass is null)
            {
                return new Result<Subscription>(CommonConsts.NOT_FOUND);
            }

            var gympassSubscription = await SubscriptionHelper.GetActiveGympassSubscriptionByExternalId(
                request.GympassId, request.ExternalSubscriptionId, _subscriptionRepository);

            var gympassValidateResult = ValidateSubscription(gympassSubscription, request);

            if (!gympassValidateResult.IsSuccess)
            {
                return gympassValidateResult;
            }

            gympassSubscription.IsActive = false;

            var updateResult = await _subscriptionRepository.UpdateSubscription(gympassSubscription.SubscriptionId, gympassSubscription);

            if (updateResult.IsSuccess)
            {
                await _subscriptionRepository.SaveChangesAsync();
            }

            return updateResult;
        }

        private Result<Subscription> ValidateSubscription(Subscription subscription, TerminateGympassSubscriptionCommand request)
        {
            if (subscription is null)
            {
                var message = $"Subscription with external id = {request.ExternalSubscriptionId} " +
                    $"and associated with gympass with id = {request.GympassId} not found";

                _logger.LogError(message);
                return new Result<Subscription>(message);
            }

            if (!subscription.IsActive)
            {
                return new Result<Subscription>($"Subscription with id = {subscription.SubscriptionId} is not active");
            }

            return new Result<Subscription>(subscription);
        }
    }
}
