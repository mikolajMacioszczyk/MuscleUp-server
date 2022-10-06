using Common.Models;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Services
{
    public class WorkerEmploymentService : IWorkerEmploymentService
    {
        private readonly IWorkerEmploymentRepository _repository;
        private readonly IFitnessClubRepository _fitnessClubRepository;

        public WorkerEmploymentService(IWorkerEmploymentRepository repository, IFitnessClubRepository fitnessClubRepository)
        {
            _repository = repository;
            _fitnessClubRepository = fitnessClubRepository;
        }

        public Task<IEnumerable<WorkerEmployment>> GetAllWorkerEmployments(string fitnessClubId) =>
            _repository.GetAllWorkerEmployments(fitnessClubId, false);

        public Task<Result<FitnessClub>> GetFitnessClubOfWorker(string workerId) =>
            _repository.GetFitnessClubOfWorker(workerId, false);

        public Task<WorkerEmployment> GetWorkerEmploymentById(string fitnessClubId, string workerId) =>
            _repository.GetWorkerEmploymentById(fitnessClubId, workerId, false);

        public async Task<Result<WorkerEmployment>> CreateWorkerEmployment(WorkerEmployment workerEmployment)
        {
            // TODO: Validate UserId
            var fitnessClubFromDb = await _fitnessClubRepository.GetById(workerEmployment.FitnessClubId, true);

            if (fitnessClubFromDb is null)
            {
                return new Result<WorkerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            workerEmployment.FitnessClub = fitnessClubFromDb;
            workerEmployment.FitnessClubId = fitnessClubFromDb.FitnessClubId;

            var createResult = await _repository.CreateWorkerEmployment(workerEmployment);

            if (createResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
