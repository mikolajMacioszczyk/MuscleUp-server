using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using MediatR;

namespace Auth.Application.Members.Commands
{
    public record UpdateMemberCommand : IRequest<MemberDto>
    {
        public UpdateMemberDto UpdateDto { get; init; }
    }

    public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto>
    {
        private readonly ISpecificUserRepository<Member, RegisterMemberDto> _repository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public UpdateMemberCommandHandler(
            ISpecificUserRepository<Member, RegisterMemberDto> repository, 
            IMapper mapper, 
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<MemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var memberId = _httpAuthContext.UserId;
            var model = _mapper.Map<Member>(request.UpdateDto);
            model.User = _mapper.Map<ApplicationUser>(request.UpdateDto);

            var memberResult = await _repository.UpdateData(memberId, model);

            if (memberResult.IsSuccess)
            {
                return _mapper.Map<MemberDto>(memberResult.Value);
            }

            throw new BadRequestException(memberResult.ErrorCombined);
        }
    }
}
