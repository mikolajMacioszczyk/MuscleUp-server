using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Commands
{
    public record TerminateWorkerEmploymentCommand : IRequest<Result<WorkerEmployment>>
    {
        public string WorkerEmploymentId { get; init; }
    }

    public class TerminateWorkerEmploymentCommandHandler : IRequestHandler<TerminateWorkerEmploymentCommand, Result<WorkerEmployment>>
    {
        private readonly IEmploymentRepository<WorkerEmployment> _repository;

        public TerminateWorkerEmploymentCommandHandler(IEmploymentRepository<WorkerEmployment> repository)
        {
            _repository = repository;
        }

        public async Task<Result<WorkerEmployment>> Handle(TerminateWorkerEmploymentCommand request, CancellationToken cancellationToken)
        {
            var terminateResult = await _repository.TerminateEmployment(request.WorkerEmploymentId);

            if (terminateResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return terminateResult;
        }
    }
}
