using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Queries
{
    public record GetFitnessClubOfWorkerQuery : IRequest<Result<FitnessClub>>
    {
        public string WorkerId { get; init; }
    }

    public class GetFitnessClubOfWorkerQueryHandler : IRequestHandler<GetFitnessClubOfWorkerQuery, Result<FitnessClub>>
    {
        private readonly IEmploymentRepository<WorkerEmployment> _employmentRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;

        public GetFitnessClubOfWorkerQueryHandler(
            IEmploymentRepository<WorkerEmployment> repository, 
            IFitnessClubRepository fitnessClubRepository)
        {
            _employmentRepository = repository;
            _fitnessClubRepository = fitnessClubRepository;
        }

        public async Task<Result<FitnessClub>> Handle(GetFitnessClubOfWorkerQuery request, CancellationToken cancellationToken)
        {
            var ownerFitnessClubs = await _fitnessClubRepository.GetOwnerFitnessClubs(request.WorkerId, onlyActive: true, asTracking: false);

            if (ownerFitnessClubs.Any())
            {
                // TODO: Return all fitness clubs. To be implemented later
                return new Result<FitnessClub>(ownerFitnessClubs.FirstOrDefault());
            }

            return await _employmentRepository.GetFitnessClubOfEmployee(request.WorkerId, false);
        }
    }
}
