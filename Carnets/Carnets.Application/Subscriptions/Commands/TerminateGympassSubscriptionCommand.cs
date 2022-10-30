using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record TerminateGympassSubscriptionCommand(string GympassId, string CustomerId, string PaymentMethodId) 
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

            var gympassSubscription = await GetActiveGympassSubscription(request.GympassId);

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

        private async Task<Subscription> GetActiveGympassSubscription(string gympassId)
        {
            var allSubscriptions = await _subscriptionRepository.GetAllGympassSubscriptions(new[] { gympassId }, true);

            return allSubscriptions.FirstOrDefault(g => g.IsActive);
        }

        private Result<Subscription> ValidateSubscription(Subscription subscription, TerminateGympassSubscriptionCommand request)
        {
            if (subscription is null)
            {
                return new Result<Subscription>($"Gympass with id: {request.GympassId} has no active subscription");
            }

            if (subscription.StripeCustomerId != request.CustomerId)
            {
                _logger.LogWarning($"Invalid StripeCustomerId. From subscription = {subscription.StripeCustomerId}, from request = {request.CustomerId}");
            }

            if (subscription.StripePaymentmethodId != request.PaymentMethodId)
            {
                _logger.LogWarning($"Invalid StripePaymentmethodId. From subscription = {subscription.StripePaymentmethodId}, from request = {request.PaymentMethodId}");
            }

            return new Result<Subscription>(subscription);
        }
    }
}
