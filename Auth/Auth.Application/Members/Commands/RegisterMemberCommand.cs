using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using MediatR;

namespace Auth.Application.Members.Commands
{
    public record RegisterMemberCommand : IRequest<MemberDto>
    {
        public RegisterMemberDto RegisterDto { get; init; }
    }

    public class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand, MemberDto>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly IMapper _mapper;

        public RegisterMemberCommandHandler(ISpecificUserRepository<Member, RegisterMemberDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MemberDto> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
        {
            var memberResult = await _repository.Register(request.RegisterDto);

            if (memberResult.IsSuccess)
            {
                return _mapper.Map<MemberDto>(memberResult.Value);
            }

            throw new BadRequestException(memberResult.ErrorCombined);
        }
    }
}
