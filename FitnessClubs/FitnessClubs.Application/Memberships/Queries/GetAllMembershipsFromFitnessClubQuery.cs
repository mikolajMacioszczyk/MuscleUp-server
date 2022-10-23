using AutoMapper;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.Memberships.Dtos;
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
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetAllMembershipsFromFitnessClubQueryHandler(
            IMembershipRepository repository,
            IAuthService authService,
            IMapper mapper)
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipDto>> Handle(GetAllMembershipsFromFitnessClubQuery request, CancellationToken cancellationToken)
        {
            var memberships = await _repository.GetAllMembershipsFromFitnessClub(request.FitnessClubId, false);
            
            var membershipDtos = _mapper.Map<IEnumerable<MembershipDto>>(memberships);

            var memberIds = memberships.Select(m => m.MemberId).ToList();

            if (memberIds.Any())
            {
                var memberDatas = await _authService.GetAllMembersWithIds(memberIds);

                if (memberDatas.IsSuccess)
                {
                    foreach (var membershipDto in membershipDtos)
                    {
                        membershipDto.MemberData = memberDatas.Value.FirstOrDefault(m => m.UserId == membershipDto.MemberId);
                    }
                }
            }

            return membershipDtos;
        }
    }
}
