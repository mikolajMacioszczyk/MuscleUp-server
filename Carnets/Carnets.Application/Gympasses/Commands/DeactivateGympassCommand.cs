using Carnets.Application.Interfaces;
using Carnets.Application.Subscriptions.Commands;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Gympasses.Commands
{
    public record DeactivateGympassCommand(string GympassId) : IRequest<Result<Gympass>>
    { }

    public class DeactivateGympassCommandHandler : IRequestHandler<DeactivateGympassCommand, Result<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<DeactivateGympassCommandHandler> _logger;

        public DeactivateGympassCommandHandler(
            IGympassRepository gympassRepository,
            ISubscriptionRepository subscriptionRepository,
            IMediator mediator,
            ILogger<DeactivateGympassCommandHandler> logger)
        {
            _gympassRepository = gympassRepository;
            _subscriptionRepository = subscriptionRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<Gympass>> Handle(DeactivateGympassCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != GympassStatus.Active && gympass.Status != GympassStatus.New)
            {
                return new Result<Gympass>($"Cannot deactivate gympass in status: {gympass.Status}");
            }

            gympass.Status = GympassStatus.Inactive;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await DeactivateGympassSubscriptions(request.GympassId);

                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }

        private async Task DeactivateGympassSubscriptions(string gympassId)
        {
            var gympassSubscriptions = await _subscriptionRepository.GetAllGympassSubscriptions(new[] { gympassId }, false);

            foreach (var activeSubscription in gympassSubscriptions.Where(g => g.IsActive))
            {
                var cancellResult = await _mediator.Send(new CancellSubscriptionCommand(activeSubscription.SubscriptionId));

                if (!cancellResult.IsSuccess)
                {
                    _logger.LogCritical($"Unable to cancell subscription with id = {activeSubscription.SubscriptionId} when deactivating Gympass");
                }
            }
        }
    }
}
