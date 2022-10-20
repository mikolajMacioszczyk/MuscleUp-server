using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Workers.Queries
{
    public record GetAllWorkersWithIdsQuery : IRequest<IEnumerable<WorkerDto>> 
    {
        public string UserIds { get; init; }
        public string Separator { get; init; }
    }

    public class GetAllWorkersWithIdsQueryHandler : IRequestHandler<GetAllWorkersWithIdsQuery, IEnumerable<WorkerDto>>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly IMapper _mapper;

        public GetAllWorkersWithIdsQueryHandler(
            ISpecificUserRepository<Worker, RegisterWorkerDto> repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerDto>> Handle(GetAllWorkersWithIdsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserIds)) throw new BadRequestException($"{nameof(request.UserIds)} cannot be empty string");
            if (string.IsNullOrEmpty(request.Separator)) throw new BadRequestException($"{nameof(request.Separator)} cannot be empty string");

            var userIds = request.UserIds.Split(request.Separator);

            var users = await _repository.GetUsersByIds(userIds);

            return _mapper.Map<IEnumerable<WorkerDto>>(users);
        }
    }
}
