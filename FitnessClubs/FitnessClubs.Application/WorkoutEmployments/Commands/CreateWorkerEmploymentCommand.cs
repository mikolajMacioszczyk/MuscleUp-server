using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Commands
{
    public record CreateWorkerEmploymentCommand : IRequest<Result<WorkerEmployment>>
    {
        public WorkerEmployment WorkerEmployment { get; init; }
    }

    public class CreateWorkerEmploymentCommandHandler : IRequestHandler<CreateWorkerEmploymentCommand, Result<WorkerEmployment>>
    {
        private readonly IWorkerEmploymentRepository _workerEmploymentRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;

        public CreateWorkerEmploymentCommandHandler(
            IFitnessClubRepository fitnessClubRepository, 
            IWorkerEmploymentRepository workerEmploymentRepository)
        {
            _fitnessClubRepository = fitnessClubRepository;
            _workerEmploymentRepository = workerEmploymentRepository;
        }

        public async Task<Result<WorkerEmployment>> Handle(CreateWorkerEmploymentCommand request, CancellationToken cancellationToken)
        {
            // TODO: Validate UserId
            var workerEmployment = request.WorkerEmployment;
            var fitnessClubFromDb = await _fitnessClubRepository.GetById(workerEmployment.FitnessClubId, true);

            if (fitnessClubFromDb is null)
            {
                return new Result<WorkerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            workerEmployment.FitnessClub = fitnessClubFromDb;
            workerEmployment.FitnessClubId = fitnessClubFromDb.FitnessClubId;

            var createResult = await _workerEmploymentRepository.CreateWorkerEmployment(workerEmployment);

            if (createResult.IsSuccess)
            {
                await _workerEmploymentRepository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
