using Carnets.Application.Gympasses.Helpers;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Enums;
using Common.Models;
using MediatR;

namespace Carnets.Application.Gympasses.Queries
{
    public record GetAllGympassesQuery : IRequest<IEnumerable<Gympass>> { }

    public class GetAllGympassesQueryHandler : IRequestHandler<GetAllGympassesQuery, IEnumerable<Gympass>>
    {
        private readonly IGympassRepository _gympassRepository;
        private HttpAuthContext _httpAuthContext;
        private readonly ISender _mediator;

        public GetAllGympassesQueryHandler(
            IGympassRepository gympassRepository,
            HttpAuthContext httpAuthContext, 
            ISender mediator)
        {
            _gympassRepository = gympassRepository;
            _httpAuthContext = httpAuthContext;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Gympass>> Handle(GetAllGympassesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Gympass> all;

            if (_httpAuthContext.UserRole == RoleType.Member)
            {
                var memberId = _httpAuthContext.UserId;
                all = await _gympassRepository.GetAllForMember(memberId, false);
            }
            else
            {
                all = await _gympassRepository.GetAll(false);
            }

            await GympassHelper.EnsureGympassActivityStatus(_mediator, all);

            return all;
        }
    }
}
