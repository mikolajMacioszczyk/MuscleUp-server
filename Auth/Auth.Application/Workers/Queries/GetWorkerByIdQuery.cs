using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Workers.Queries
{
    public record GetWorkerByIdQuery(string WorkerId) : IRequest<WorkerDto> { }

    public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDto>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly IMapper _mapper;

        public GetWorkerByIdQueryHandler(
            ISpecificUserRepository<Worker, RegisterWorkerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WorkerDto> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = await _repository.GetById(request.WorkerId);

            if (worker is null)
            {
                return null;
            }

            return _mapper.Map<WorkerDto>(worker);
        }
    }
}
