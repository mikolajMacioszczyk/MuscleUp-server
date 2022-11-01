using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Members.Queries
{
    public record GetMemberByIdQuery(string MemberId) : IRequest<MemberDto> { }

    public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly IMapper _mapper;

        public GetMemberByIdQueryHandler(
            ISpecificUserRepository<Member, RegisterMemberDto> repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await _repository.GetById(request.MemberId);

            if (member is null)
            {
                return null;
            }

            return _mapper.Map<MemberDto>(member);
        }
    }
}
