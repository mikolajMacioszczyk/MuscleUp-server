using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CreateSubscriptionCommand(string GympassId, string CustomerId, string PaymentMethodId, string ExternalSubscriptionId)
        : IRequest<Result<Subscription>>
    { }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result<Subscription>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<CreateSubscriptionCommandHandler> _logger;

        public CreateSubscriptionCommandHandler(
            IGympassRepository gympassRepository,
            ISubscriptionRepository subscriptionRepository,
            ILogger<CreateSubscriptionCommandHandler> logger)
        {
            _gympassRepository = gympassRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                _logger.LogWarning($"Cannot create subscription for not existing gympass: {request.GympassId}");
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            var subscription = await CreateSubscription(request, gympass);

            // save changes
            try
            {
                await _subscriptionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }

            return new Result<Subscription>(subscription);
        }

        private async Task<Subscription> CreateSubscription(CreateSubscriptionCommand request, Gympass gympass)
        {
            var subscription = new Subscription()
            {
                StripeCustomerId = string.IsNullOrEmpty(request.CustomerId) ? string.Empty : request.CustomerId,
                Gympass = gympass,
                StripePaymentmethodId = string.IsNullOrEmpty(request.PaymentMethodId) ? string.Empty : request.PaymentMethodId,
                ExternalSubscriptionId = string.IsNullOrEmpty(request.ExternalSubscriptionId) ? string.Empty : request.ExternalSubscriptionId,
                CreationDate = DateTime.UtcNow,
                LastPaymentDate = DateTime.UtcNow,
                IsActive = true
            };
            var createSubscriptionResult = await _subscriptionRepository.CreateSubscription(subscription);

            if (createSubscriptionResult.IsSuccess)
            {
                return createSubscriptionResult.Value;
            }

            _logger.LogError($"Creation subscription failed");
            throw new ApplicationException();
        }
    }
}
