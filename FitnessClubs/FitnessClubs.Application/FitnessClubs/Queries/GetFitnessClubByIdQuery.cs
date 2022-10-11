using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Queries
{
    public record GetFitnessClubByIdQuery : IRequest<FitnessClub> 
    {
        public string FitnessClubId { get; init; }
    }

    public class GetFitnessClubByIdQueryHandler : IRequestHandler<GetFitnessClubByIdQuery, FitnessClub>
    {
        private readonly IFitnessClubRepository _repository;

        public GetFitnessClubByIdQueryHandler(IFitnessClubRepository repository)
        {
            _repository = repository;
        }

        public Task<FitnessClub> Handle(GetFitnessClubByIdQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetById(request.FitnessClubId, false);
        }
    }
}
