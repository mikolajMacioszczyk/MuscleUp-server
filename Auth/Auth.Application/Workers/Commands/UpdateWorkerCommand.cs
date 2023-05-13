using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Auth.Application.Workers.Commands
{
    public record UpdateWorkerCommand(string WorkerId, UpdateWorkerDto UpdateDto) : IRequest<WorkerDto>
    { }

    public class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, WorkerDto>
    {
        private readonly ISpecificUserRepository<Worker, RegisterWorkerDto> _repository;
        private readonly IMapper _mapper;

        public UpdateWorkerCommandHandler(
            ISpecificUserRepository<Worker, RegisterWorkerDto> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WorkerDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Worker>(request.UpdateDto);
            model.User = _mapper.Map<ApplicationUser>(request.UpdateDto);

            var workerResult = await _repository.UpdateData(request.WorkerId, model);

            if (workerResult.IsSuccess)
            {
                return _mapper.Map<WorkerDto>(workerResult.Value);
            }

            throw new InvalidInputException(workerResult.ErrorCombined);
        }
    }
}
