using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetActiveMemberGympassesQuery(string MemberId, string FitnessClubId) : IRequest<IEnumerable<Gympass>> { }

    public class GetActiveMemberGympassesQueryHandler : IRequestHandler<GetActiveMemberGympassesQuery, IEnumerable<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;

        public GetActiveMemberGympassesQueryHandler(
            IGympassRepository gympassRepository)
        {
            _gympassRepository = gympassRepository;
        }

        public async Task<IEnumerable<Gympass>> Handle(GetActiveMemberGympassesQuery request, CancellationToken cancellationToken)
        {
            var memberGympasses = await _gympassRepository.GetAllForMember(request.MemberId, false);

            return memberGympasses
                .Where(g => g.Status == Domain.Enums.GympassStatus.Active)
                .Where(g => g.GympassType.FitnessClubId == request.FitnessClubId);
        }
    }
}
