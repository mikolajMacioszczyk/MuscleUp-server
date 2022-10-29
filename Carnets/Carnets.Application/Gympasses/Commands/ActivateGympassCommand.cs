using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Commands
{
    public record ActivateGympassCommand : IRequest<Result<Gympass>>
    {
        public string GympassId { get; init; }

        public bool SaveChanges { get; init; } = true;
    }

    public class ActivateGympassCommandHandler : IRequestHandler<ActivateGympassCommand, Result<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;

        public ActivateGympassCommandHandler(IGympassRepository gympassRepository)
        {
            _gympassRepository = gympassRepository;
        }

        public async Task<Result<Gympass>> Handle(ActivateGympassCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != GympassStatus.New && gympass.Status != GympassStatus.Inactive)
            {
                return new Result<Gympass>($"Cannot activate gympass in status: {gympass.Status}");
            }

            if (gympass.RemainingValidityPeriodInSeconds <= 0 && gympass.RemainingEntries <= 0)
            {
                return new Result<Gympass>($"The Gympass validity has ended");
            }

            var now = DateTime.UtcNow;
            if (gympass.Status == GympassStatus.New)
            {
                gympass.ActivationDate = now;
            }
            gympass.ValidityDate = now.AddSeconds(gympass.RemainingValidityPeriodInSeconds);

            gympass.Status = GympassStatus.Active;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess && request.SaveChanges)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
