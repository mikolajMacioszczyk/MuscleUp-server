using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Commands
{
    public record DeactivateGympassCommand(string GympassId) : IRequest<Result<Gympass>>
    { }

    public class DeactivateGympassCommandHandler : IRequestHandler<DeactivateGympassCommand, Result<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;

        public DeactivateGympassCommandHandler(IGympassRepository gympassRepository)
        {
            _gympassRepository = gympassRepository;
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
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
