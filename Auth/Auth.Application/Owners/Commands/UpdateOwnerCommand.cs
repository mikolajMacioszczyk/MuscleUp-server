using Auth.Application.Common.Interfaces;
using Auth.Application.Owners.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using MediatR;

namespace Auth.Application.Owners.Commands
{
    public record UpdateOwnerCommand(string OwnerId, UpdateOwnerDto UpdateDto) : IRequest<OwnerDto>
    { }

    public class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand, OwnerDto>
    {
        private readonly ISpecificUserRepository<Owner, RegisterOwnerDto> _repository;
        private readonly IMapper _mapper;

        public UpdateOwnerCommandHandler(
            ISpecificUserRepository<Owner, RegisterOwnerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnerDto> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Owner>(request.UpdateDto);
            model.User = _mapper.Map<ApplicationUser>(request.UpdateDto);

            var ownerResult = await _repository.UpdateData(request.OwnerId, model);

            if (ownerResult.IsSuccess)
            {
                return _mapper.Map<OwnerDto>(ownerResult.Value);
            }

            throw new BadRequestException(ownerResult.ErrorCombined);
        }
    }
}
