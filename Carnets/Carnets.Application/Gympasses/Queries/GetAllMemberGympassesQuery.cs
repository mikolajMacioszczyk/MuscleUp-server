using Carnets.Application.Gympasses.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetAllMemberGympassesQuery(string MemberId) : IRequest<IEnumerable<Gympass>> { }

    public class GetAllMemberGympassesQueryHandler : IRequestHandler<GetAllMemberGympassesQuery, IEnumerable<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;
        private readonly ISender _mediator;

        public GetAllMemberGympassesQueryHandler(
            IGympassRepository gympassRepository,
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Gympass>> Handle(GetAllMemberGympassesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Gympass> all = await _gympassRepository.GetAllForMember(request.MemberId, true);

            await GympassHelper.EnsureGympassActivityStatus(_mediator, all);

            return all;
        }
    }
}
