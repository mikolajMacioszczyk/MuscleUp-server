using Auth.Application.Common.Interfaces;
using Auth.Application.Owners.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auth.Application.Owners.Queries
{
    public record GetAllOwnersQuery : IRequest<IEnumerable<OwnerDto>> { }

    public class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, IEnumerable<OwnerDto>>
    {
        private readonly ISpecificUserRepository<Owner, RegisterOwnerDto> _repository;
        private readonly IMapper _mapper;

        public GetAllOwnersQueryHandler(ISpecificUserRepository<Owner, RegisterOwnerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OwnerDto>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<OwnerDto>>(await _repository.GetAll());
        }
    }
}
