using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auth.Application.Workers.Queries
{
    public record GetAllWorkersQuery : IRequest<IEnumerable<WorkerDto>> { }

    public class GetAllWorkersQueryHandler : IRequestHandler<GetAllWorkersQuery, IEnumerable<WorkerDto>>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly IMapper _mapper;

        public GetAllWorkersQueryHandler(ISpecificUserRepository<Worker, RegisterWorkerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerDto>> Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<WorkerDto>>(await _repository.GetAll());
        }
    }
}
