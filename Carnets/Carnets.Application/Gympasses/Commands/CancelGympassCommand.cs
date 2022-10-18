using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Commands
{
    public record CancelGympassCommand : IRequest<Result<Gympass>>
    {
        public string GympassId { get; init; }
    }

    public class CancelGympassCommandHandler : IRequestHandler<CancelGympassCommand, Result<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;

        public CancelGympassCommandHandler(IGympassRepository gympassRepository)
        {
            _gympassRepository = gympassRepository;
        }

        public async Task<Result<Gympass>> Handle(CancelGympassCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.Status != GympassStatus.New)
            {
                return new Result<Gympass>($"Cannot cancell gympass in status: {gympass.Status}");
            }

            gympass.Status = GympassStatus.Cancelled;

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
