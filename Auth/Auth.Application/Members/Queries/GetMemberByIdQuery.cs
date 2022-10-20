using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Members.Queries
{
    public record GetMemberByIdQuery : IRequest<MemberDto> { }

    public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public GetMemberByIdQueryHandler(
            ISpecificUserRepository<Member, RegisterMemberDto> repository, 
            IMapper mapper, 
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await _repository.GetById(_httpAuthContext.UserId);

            if (member is null)
            {
                return null;
            }

            return _mapper.Map<MemberDto>(member);
        }
    }
}
