using Carnets.Application.Interfaces;
using Carnets.Domain.Enums;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Commands
{
    public record ReduceGympassEntriesCommand : IRequest<Result<Gympass>>
    {
        public string GympassId { get; init; }
    }

    public class ReduceGympassEntriesCommandHandler : IRequestHandler<ReduceGympassEntriesCommand, Result<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;

        public ReduceGympassEntriesCommandHandler(IGympassRepository gympassRepository)
        {
            _gympassRepository = gympassRepository;
        }

        public async Task<Result<Gympass>> Handle(ReduceGympassEntriesCommand request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassRepository.GetById(request.GympassId, true);

            if (gympass is null)
            {
                return new Result<Gympass>(Common.CommonConsts.NOT_FOUND);
            }

            if (gympass.GympassType.ValidationType != GympassTypeValidation.Entries)
            {
                return new Result<Gympass>($"Cannot update entries of gympass with validation type \"{gympass.GympassType.ValidationType}\"");
            }

            gympass.RemainingEntries = gympass.RemainingEntries - 1;

            if (gympass.RemainingEntries < 0)
            {
                return new Result<Gympass>($"Remaining gympass entries cannot be less than 0");
            }

            var updateResult = await _gympassRepository.UpdateGympass(gympass);

            if (updateResult.IsSuccess)
            {
                await _gympassRepository.SaveChangesAsync();
            }

            return updateResult;
        }
    }
}
