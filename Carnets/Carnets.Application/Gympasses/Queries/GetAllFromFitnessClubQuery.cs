using Carnets.Application.Gympasses.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetAllFromFitnessClubQuery : IRequest<IEnumerable<Gympass>>
    {
        public string FitnessClubId { get; init; }
    }

    public class GetAllFromFitnessClubQueryHandler : IRequestHandler<GetAllFromFitnessClubQuery, IEnumerable<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISender _mediator;

        public GetAllFromFitnessClubQueryHandler(
            IGympassRepository gympassRepository,
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Gympass>> Handle(GetAllFromFitnessClubQuery request, CancellationToken cancellationToken)
        {
            var all = await _gympassRepository.GetAllFromFitnessClub(request.FitnessClubId, false);

            await GympassHelper.EnsureGympassActivityStatus(_mediator, all);

            return all;
        }
    }
}
