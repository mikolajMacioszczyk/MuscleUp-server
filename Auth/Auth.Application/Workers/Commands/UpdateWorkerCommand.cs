using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using MediatR;

namespace Auth.Application.Workers.Commands
{
    public record UpdateWorkerCommand : IRequest<WorkerDto>
    {
        public UpdateWorkerDto UpdateDto { get; init; }
    }

    public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly HttpAuthContext _httpAuthContext;
        private readonly IMapper _mapper;

        public UpdateWorkerCommandHandler(
            ISpecificUserRepository<Worker, RegisterWorkerDto> repository,
            IMapper mapper,
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<WorkerDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
        {
            var workerId = _httpAuthContext.UserId;
            var model = _mapper.Map<Worker>(request.UpdateDto);
            model.User = _mapper.Map<ApplicationUser>(request.UpdateDto);

            var workerResult = await _repository.UpdateData(workerId, model);

            if (workerResult.IsSuccess)
            {
                return _mapper.Map<WorkerDto>(workerResult.Value);
            }

            throw new BadRequestException(workerResult.ErrorCombined);
        }
    }
}
