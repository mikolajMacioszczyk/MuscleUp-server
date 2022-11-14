using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CreateOrExtendGympassSubscriptionByExternalIdCommand(string CustomerId, string PaymentMethodId, string ExternalSubscriptionId)
        : IRequest<Result<Subscription>>
    { }

    public class CreateOrExtendGympassSubscriptionByExternalIdCommandHandler : IRequestHandler<CreateOrExtendGympassSubscriptionByExternalIdCommand, Result<Subscription>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<CreateOrExtendGympassSubscriptionCommand> _logger;
        private readonly ISender _mediator;

        public CreateOrExtendGympassSubscriptionByExternalIdCommandHandler(
            IGympassRepository gympassRepository,
            ISubscriptionRepository subscriptionRepository,
            ILogger<CreateOrExtendGympassSubscriptionCommand> logger,
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Result<Subscription>> Handle(CreateOrExtendGympassSubscriptionByExternalIdCommand request, CancellationToken cancellationToken)
        {
            Subscription existingSubscription = await _subscriptionRepository.GetSubscription(
                s => s.ExternalSubscriptionId == request.ExternalSubscriptionId && s.IsActive, 
                asTracking: true);

            if (existingSubscription is null)
            {
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            existingSubscription = await RefreshSubscriptionPaymentDate(existingSubscription);
            var gympass = existingSubscription.Gympass;

            // first activation
            if (gympass.Status == GympassStatus.New)
            {
                // activate gympass
                var activationResult = await _mediator.Send(new ActivateGympassCommand(gympass.GympassId, false));

                if (!activationResult.IsSuccess)
                {
                    _logger.LogWarning($"Cannot activate gympass: {gympass.GympassId}");
                    return new Result<Subscription>(activationResult.Errors);
                }

                gympass = activationResult.Value;

                // TODO: send email to customer by notifications
            }
            // already active gympass
            else
            {
                gympass = await ExtendGympassValidity(gympass);
            }

            // save changes
            try
            {
                await _subscriptionRepository.SaveChangesAsync();
                await _gympassRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }

            return new Result<Subscription>(existingSubscription);
        }

        private async Task<Subscription> RefreshSubscriptionPaymentDate(Subscription subscription)
        {
            subscription.LastPaymentDate = DateTime.UtcNow;

            var updateSubscriptionResult = await _subscriptionRepository.UpdateSubscription(subscription.SubscriptionId, subscription);

            if (updateSubscriptionResult.IsSuccess)
            {
                return updateSubscriptionResult.Value;
            }

            _logger.LogError($"Update subscription failed: {subscription.SubscriptionId}");
            throw new ApplicationException();
        }

        private async Task<Gympass> ExtendGympassValidity(Gympass gympass)
        {
            switch (gympass.GympassType.ValidationType)
            {
                case GympassTypeValidation.Time:
                    ExtendGympassValidityDate(gympass);
                    break;
                case GympassTypeValidation.Entries:
                    // sets entries to default value
                    gympass.RemainingEntries = gympass.GympassType.AllowedEntries;
                    ExtendGympassValidityDate(gympass);
                    break;
                default:
                    _logger.LogError($"Unhandled validation type: {gympass.GympassType.ValidationType}");
                    break;
            }

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                return updateResult.Value;
            }

            _logger.LogError($"Cannot update gympass validity: {gympass.GympassId}");
            throw new ApplicationException();
        }

        private void ExtendGympassValidityDate(Gympass gympass)
        {
            var intervalCount = gympass.GympassType.IntervalCount;
            var newDate = gympass.GympassType.Interval.AddToDate(gympass.ValidityDate, intervalCount);

            gympass.ValidityDate = newDate;
        }
    }
}
