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
        private readonly ILogger<EnsureGympassActivityStatusCommandHandler> _logger;

        public EnsureGympassActivityStatusCommandHandler(IGympassRepository gympassRepository, 
            ILogger<EnsureGympassActivityStatusCommandHandler> logger)
        {
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

            if (request.Gympass.ValidityDate < DateTime.UtcNow)
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
    }
}
