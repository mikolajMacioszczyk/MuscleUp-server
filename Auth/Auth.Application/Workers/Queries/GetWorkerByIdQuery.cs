using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models;
using MediatR;

namespace Auth.Application.Workers.Queries
{
    public record GetWorkerByIdQuery : IRequest<WorkerDto> { }

    public class GetWorkerByIdQueryHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDto>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public GetWorkerByIdQueryHandler(
            ISpecificUserRepository<Worker, RegisterWorkerDto> repository,
            IMapper mapper,
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<WorkerDto> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = await _repository.GetById(_httpAuthContext.UserId);

            if (worker is null)
            {
                return null;
            }

            return _mapper.Map<WorkerDto>(worker);
        }
    }
}
