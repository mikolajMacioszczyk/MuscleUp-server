using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CancellSubscriptionCommand(string SubscriptionId) : IRequest<Result<Subscription>>
    { }

    public class CancellSubscriptionCommandHandler : IRequestHandler<CancellSubscriptionCommand, Result<Subscription>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<CancellSubscriptionCommandHandler> _logger;
        private readonly IPaymentService _paymentService;

        public CancellSubscriptionCommandHandler(
            ISubscriptionRepository subscriptionRepository,
            ILogger<CancellSubscriptionCommandHandler> logger, 
            IPaymentService paymentService)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
            _paymentService = paymentService;
        }

        public async Task<Result<Subscription>> Handle(CancellSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionById(request.SubscriptionId, true);

            if (subscription is null)
            {
                return new Result<Subscription>($"Not found subscription with id {request.SubscriptionId}");
            }

            if (!subscription.IsActive)
            {
                return new Result<Subscription>($"Subscription with id {request.SubscriptionId} already cancelled");
            }

            subscription.IsActive = false;
            var updateResult = await _subscriptionRepository.UpdateSubscription(subscription.SubscriptionId, subscription);

            if (!updateResult.IsSuccess)
            {
                return new Result<Subscription>(updateResult.ErrorCombined);
            }

            try
            {
                await _subscriptionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return updateResult;
            }

            await CancellSubscriptionOnExternalServive(updateResult.Value);

            return updateResult;
        }

        private async Task CancellSubscriptionOnExternalServive(Subscription subscription)
        {
            if (!string.IsNullOrEmpty(subscription.ExternalSubscriptionId))
            {
                await _paymentService.CancelSubscription(subscription.ExternalSubscriptionId);
                _logger.LogInformation($"Cancelled subscription (id = {subscription.SubscriptionId}) " +
                    $"associated with an external payment system (externalId = {subscription.ExternalSubscriptionId})");
            }
            else
            {
                _logger.LogInformation($"Cancelled subscription (id = {subscription.SubscriptionId}) " +
                    $"not associated with an external payment system");
            }
        }
    }
}
