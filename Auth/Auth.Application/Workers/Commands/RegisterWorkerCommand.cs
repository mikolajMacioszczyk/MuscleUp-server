using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Workers.Commands
{
    public record RegisterWorkerCommand : IRequest<WorkerDto>
    {
        public RegisterWorkerDto RegisterDto { get; init; }
    }

    public class RegisterWorkerCommandHandler : IRequestHandler<RegisterWorkerCommand, WorkerDto>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly IMapper _mapper;

        public RegisterWorkerCommandHandler(ISpecificUserRepository<Worker, RegisterWorkerDto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WorkerDto> Handle(RegisterWorkerCommand request, CancellationToken cancellationToken)
        {
            var workerResult = await _repository.Register(request.RegisterDto);

            if (workerResult.IsSuccess)
            {
                return _mapper.Map<WorkerDto>(workerResult.Value);
            }

            throw new BadRequestException(workerResult.ErrorCombined);
        }
    }
}
