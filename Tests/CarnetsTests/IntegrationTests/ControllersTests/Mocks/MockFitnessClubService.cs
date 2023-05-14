using Carnets.Application.Interfaces;
using Common.Consts;
using Common.Models.Dtos;
using Common.Models;

namespace CarnetsTests.IntegrationTests.ControllersTests.Mocks
{
    public class MockFitnessClubService : IFitnessClubHttpService
    {
        public Task<Result<FitnessClubDto>> GetFitnessClubById(string fitnessClubId)
        {
            if (fitnessClubId == SeedConsts.DefaultFitnessClubId)
            {
                return Task.FromResult(new Result<FitnessClubDto>(new FitnessClubDto()
                {
                    FitnessClubId = SeedConsts.DefaultFitnessClubId
                }));
            }

            return Task.FromResult(new Result<FitnessClubDto>(Common.CommonConsts.NOT_FOUND));
        }

        public Task<Result<FitnessClubDto>> GetFitnessClubOfWorker(string workerId)
        {
            if (workerId == SeedConsts.DefaultWorkerId || workerId == SeedConsts.DefaultOwnerId)
            {
                return Task.FromResult(new Result<FitnessClubDto>(new FitnessClubDto()
                {
                    FitnessClubId = SeedConsts.DefaultFitnessClubId
                }));
            }

            return Task.FromResult(new Result<FitnessClubDto>(Common.CommonConsts.NOT_FOUND));
        }
    }
}
