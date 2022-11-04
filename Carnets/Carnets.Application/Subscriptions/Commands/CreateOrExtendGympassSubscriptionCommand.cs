using Carnets.Application.Gympasses.Commands;
using Carnets.Application.Interfaces;
using Carnets.Application.Subscriptions.Helpers;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CreateOrExtendGympassSubscriptionCommand(string GympassId, string CustomerId, string PaymentMethodId, string ExternalSubscriptionId)
        : IRequest<Result<Subscription>>
    { }

    public class CreateOrExtendGympassSubscriptionCommandHandler : IRequestHandler<CreateOrExtendGympassSubscriptionCommand, Result<Subscription>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<CreateOrExtendGympassSubscriptionCommand> _logger;
        private readonly ISender _mediator;

        public CreateOrExtendGympassSubscriptionCommandHandler(
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

        public async Task<Result<Subscription>> Handle(CreateOrExtendGympassSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                _logger.LogWarning($"Cannot create subscription for not existing gympass: {request.GympassId}");
                return new Result<Subscription>(Common.CommonConsts.NOT_FOUND);
            }

            Subscription subscription = null;

            // first activation
            if (gympass.Status == GympassStatus.New)
            {
                subscription = await CreateSubscription(request, gympass);

                // activate gympass
                var activationResult = await _mediator.Send(new ActivateGympassCommand(gympass.GympassId, false));

                if (!activationResult.IsSuccess)
                {
                    _logger.LogWarning($"Cannot activate gympass: {request.GympassId}");
                    return new Result<Subscription>(activationResult.Errors);
                }

                gympass = activationResult.Value;

                // TODO: send email to customer by notifications
            }
            // already active gympass
            else
            {
                var existingSubscription = await SubscriptionHelper.GetActiveGympassSubscriptionByExternalId(
                    gympass.GympassId, request.ExternalSubscriptionId, _subscriptionRepository);

                if (existingSubscription is null)
                {
                    subscription = await CreateSubscription(request, gympass);
                }
                else
                {
                    subscription = await RefreshSubscriptionPaymentDate(existingSubscription);
                }

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

            return new Result<Subscription>(subscription);
        }

        private async Task<Subscription> CreateSubscription(CreateOrExtendGympassSubscriptionCommand request, Gympass gympass)
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
