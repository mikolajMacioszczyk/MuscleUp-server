using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Commands
{
    public record TerminateTrainerEmploymentCommand : IRequest<Result<TrainerEmployment>>
    {
        public string EmploymentId { get; init; }
    }

    public class TerminateTrainerEmploymentCommandHandler : IRequestHandler<TerminateTrainerEmploymentCommand, Result<TrainerEmployment>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;

        public TerminateTrainerEmploymentCommandHandler(IEmploymentRepository<TrainerEmployment> repository)
        {
            _repository = repository;
        }

        public async Task<Result<TrainerEmployment>> Handle(TerminateTrainerEmploymentCommand request, CancellationToken cancellationToken)
        {
            var terminateResult = await _repository.TerminateEmployment(request.EmploymentId);

            if (terminateResult.IsSuccess)
            {
                // TODO: Use broker to deactivate worker account
                await _repository.SaveChangesAsync();
            }

            return terminateResult;
        }
    }
}
