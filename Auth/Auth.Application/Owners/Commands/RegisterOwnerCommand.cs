using Auth.Application.Common.Interfaces;
using Auth.Application.Owners.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Owners.Commands
{
    public record RegisterOwnerCommand(RegisterOwnerDto RegisterDto) : IRequest<OwnerDto>
    {}

    public class RegisterOwnerCommandHandler : IRequestHandler<RegisterOwnerCommand, OwnerDto>
    {
        private readonly ISpecificUserRepository<Owner, RegisterOwnerDto> _repository;
        private readonly IMapper _mapper;

        public RegisterOwnerCommandHandler(ISpecificUserRepository<Owner, RegisterOwnerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnerDto> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
        {
            var ownerResult = await _repository.Register(request.RegisterDto);

            if (ownerResult.IsSuccess)
            {
                return _mapper.Map<OwnerDto>(ownerResult.Value);
            }

            throw new BadRequestException(ownerResult.ErrorCombined);
        }
    }
}
