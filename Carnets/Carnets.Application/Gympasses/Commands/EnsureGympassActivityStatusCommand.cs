using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Gympasses.Commands
{
    public record EnsureGympassActivityStatusCommand : IRequest<bool>
    {
        public Gympass Gympass { get; init; }

        public bool SaveChanges { get; init; } = true;
    }

    public class EnsureGympassActivityStatusCommandHandler : IRequestHandler<EnsureGympassActivityStatusCommand, bool>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<EnsureGympassActivityStatusCommandHandler> _logger;

        public EnsureGympassActivityStatusCommandHandler(
            ILogger<EnsureGympassActivityStatusCommandHandler> logger,
            IGympassRepository gympassRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _gympassRepository = gympassRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(EnsureGympassActivityStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Gympass.GympassType is null)
            {
                throw new ArgumentException(nameof(request.Gympass.GympassType));
            }

            if (request.Gympass.Status != GympassStatus.Active)
            {
                return true;
            }

            if (request.Gympass.ValidityDate < DateTime.UtcNow
                && await DoNotHaveActiveSubscription(request.Gympass.GympassId))
            {
                request.Gympass.Status = GympassStatus.Completed;
                request.Gympass.RemainingEntries = 0;

                var updateResult = await _gympassRepository.UpdateGympass(request.Gympass);

                if (updateResult.IsSuccess)
                {
                    if (request.SaveChanges)
                    {
                        await _gympassRepository.SaveChangesAsync();
                    }
                }
                else
                {
                    _logger.LogCritical(updateResult.ErrorCombined);
                }

                // TODO: Send email by notification service
            }

            return true;
        }

        private async Task<bool> DoNotHaveActiveSubscription(string gympassId)
        {
            var allSubscriptions = await _subscriptionRepository.GetAllGympassSubscriptions(new string[] { gympassId }, false);

            return !(allSubscriptions.Where(s => s.IsActive).Any());
        }
    }
}
