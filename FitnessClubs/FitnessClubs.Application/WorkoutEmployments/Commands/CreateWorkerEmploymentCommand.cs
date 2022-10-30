using Common.Interfaces;
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
        private readonly IEmploymentRepository<WorkerEmployment> _workerEmploymentRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IAuthService _authService;

        public CreateWorkerEmploymentCommandHandler(
            IFitnessClubRepository fitnessClubRepository,
            IEmploymentRepository<WorkerEmployment> workerEmploymentRepository,
            IAuthService authService)
        {
            _fitnessClubRepository = fitnessClubRepository;
            _workerEmploymentRepository = workerEmploymentRepository;
            _authService = authService;
        }

        public async Task<Result<WorkerEmployment>> Handle(CreateWorkerEmploymentCommand request, CancellationToken cancellationToken)
        {
            if (!(await _authService.DoesWorkerExists(request.WorkerEmployment.UserId)))
            {
                return new Result<WorkerEmployment>($"Worker with id {request.WorkerEmployment.UserId} does not exists");
            }

            var workerEmployment = request.WorkerEmployment;
            var fitnessClubFromDb = await _fitnessClubRepository.GetById(workerEmployment.FitnessClubId, true);

            if (fitnessClubFromDb is null)
            {
                return new Result<WorkerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            workerEmployment.FitnessClub = fitnessClubFromDb;
            workerEmployment.FitnessClubId = fitnessClubFromDb.FitnessClubId;

            var createResult = await _workerEmploymentRepository.CreateEmployment(workerEmployment);

            if (createResult.IsSuccess)
            {
                await _workerEmploymentRepository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
