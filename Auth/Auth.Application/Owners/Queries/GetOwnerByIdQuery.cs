using Auth.Application.Common.Interfaces;
using Auth.Application.Owners.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Owners.Queries
{
    public record GetOwnerByIdQuery(string OwnerId) : IRequest<OwnerDto> { }

    public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, OwnerDto>
    {
        private readonly ISpecificUserRepository<Owner, RegisterOwnerDto> _repository;
        private readonly IMapper _mapper;

        public GetOwnerByIdQueryHandler(
            ISpecificUserRepository<Owner, RegisterOwnerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnerDto> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var owner = await _repository.GetById(request.OwnerId);

            if (owner is null)
            {
                return null;
            }

            return _mapper.Map<OwnerDto>(owner);
        }
    }
}
