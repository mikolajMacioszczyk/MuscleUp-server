using Common.Models;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Domain.Services
{
    public class FitnessClubService : IFitnessClubService
    {
        private readonly IFitnessClubRepository _repository;

        public FitnessClubService(IFitnessClubRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<FitnessClub>> GetAll() =>
            _repository.GetAll(false);

        public Task<FitnessClub> GetById(string fitnessClubId) =>
            _repository.GetById(fitnessClubId, false);

        public async Task<Result<FitnessClub>> Create(FitnessClub fitnessClub)
        {
            var createResult = await _repository.Create(fitnessClub);

            if (createResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return createResult;
        }

        public async Task<Result<bool>> Delete(string fitnessClubId)
        {
            var deleteResult = await _repository.Delete(fitnessClubId);

            if (deleteResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return deleteResult;
        }
    }
}
