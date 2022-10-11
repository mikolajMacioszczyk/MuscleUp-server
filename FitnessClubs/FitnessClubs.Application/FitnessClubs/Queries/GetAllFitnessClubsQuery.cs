using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Queries
{
    public record GetAllFitnessClubsQuery : IRequest<IEnumerable<FitnessClub>> {}

    public class GetAllFitnessClubsQueryHandler : IRequestHandler<GetAllFitnessClubsQuery, IEnumerable<FitnessClub>>
    {
        private readonly IFitnessClubRepository _repository;

        public GetAllFitnessClubsQueryHandler(IFitnessClubRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<FitnessClub>> Handle(GetAllFitnessClubsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAll(false);
        }
    }
}
