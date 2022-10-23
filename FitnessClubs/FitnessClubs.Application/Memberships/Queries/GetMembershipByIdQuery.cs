using AutoMapper;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.Memberships.Dtos;
using MediatR;

namespace FitnessClubs.Application.Memberships.Queries
{
    public record GetMembershipByIdQuery : IRequest<MembershipDto>
    {
        public string MemberId { get; init; }
        public string FitnessClubId { get; init; }
    }

    public class GetMembershipByIdQueryHandler : IRequestHandler<GetMembershipByIdQuery, MembershipDto>
    {
        private readonly IMembershipRepository _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetMembershipByIdQueryHandler(
            IMembershipRepository repository,
            IAuthService authService,
            IMapper mapper)
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<MembershipDto> Handle(GetMembershipByIdQuery request, CancellationToken cancellationToken)
        {
            var membership = await _repository.GetMembershipById(request.MemberId, request.FitnessClubId, false);
            
            if (membership is null)
            {
                return null;
            }

            var membershipDto = _mapper.Map<MembershipDto>(membership);

            var memberData = await _authService.GetAllMembersWithIds(new string[] { request.MemberId });

            if (memberData.IsSuccess && memberData.Value.Any())
            {
                membershipDto.MemberData = memberData.Value.First();
            }

            return membershipDto;
        }
    }
}