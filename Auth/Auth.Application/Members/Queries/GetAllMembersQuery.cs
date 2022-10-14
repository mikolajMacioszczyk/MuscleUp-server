using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auth.Application.Members.Queries
{
    public record GetAllMembersQuery : IRequest<IEnumerable<MemberDto>> { }

    public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<MemberDto>>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly IMapper _mapper;

        public GetAllMembersQueryHandler(ISpecificUserRepository<Member, RegisterMemberDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<MemberDto>>(await _repository.GetAll());
        }
    }
}
