using AutoMapper;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.Memberships.Dtos;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.Memberships.Queries
{
    public record GetAllMembershipsFromFitnessClubQuery : IRequest<IEnumerable<MembershipDto>>
    {
        public string FitnessClubId { get; init; }
    }

    public class GetAllMembershipsFromFitnessClubQueryHandler : IRequestHandler<GetAllMembershipsFromFitnessClubQuery, IEnumerable<MembershipDto>>
    {
        private readonly IMembershipRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMembershipsFromFitnessClubQueryHandler(
            IMembershipRepository repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipDto>> Handle(GetAllMembershipsFromFitnessClubQuery request, CancellationToken cancellationToken)
        {
            var memberships = await _repository.GetAllMembershipsFromFitnessClub(request.FitnessClubId, false);
            return _mapper.Map<IEnumerable<MembershipDto>>(memberships);
        }
    }
}
