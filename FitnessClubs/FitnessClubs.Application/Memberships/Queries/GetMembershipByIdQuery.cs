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
        private readonly IMapper _mapper;

        public GetMembershipByIdQueryHandler(
            IMembershipRepository repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MembershipDto> Handle(GetMembershipByIdQuery request, CancellationToken cancellationToken)
        {
            var membership = await _repository.GetMembershipById(request.MemberId, request.FitnessClubId, false);
            
            if (membership is null)
            {
                return null;
            }

            return _mapper.Map<MembershipDto>(membership);
        }
    }
}