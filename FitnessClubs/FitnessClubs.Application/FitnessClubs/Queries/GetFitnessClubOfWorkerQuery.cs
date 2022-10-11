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
        private readonly IWorkerEmploymentRepository _repository;

        public GetFitnessClubOfWorkerQueryHandler(IWorkerEmploymentRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<FitnessClub>> Handle(GetFitnessClubOfWorkerQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetFitnessClubOfWorker(request.WorkerId, false);
        }
    }
}
