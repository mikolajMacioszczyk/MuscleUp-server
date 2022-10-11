using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Queries
{
    public record GetAllWorkerEmploymentsQuery : IRequest<IEnumerable<WorkerEmployment>> 
    {
        public string FitnessClubId { get; init; }
        public bool IncludeInactive { get; init; }
    }

    public class GetAllFitnessClubsQueryHandler : IRequestHandler<GetAllWorkerEmploymentsQuery, IEnumerable<WorkerEmployment>>
    {
        private readonly IWorkerEmploymentRepository _repository;

        public GetAllFitnessClubsQueryHandler(IWorkerEmploymentRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<WorkerEmployment>> Handle(GetAllWorkerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAllWorkerEmployments(request.FitnessClubId, request.IncludeInactive, false);
        }
    }
}
