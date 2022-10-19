using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Queries
{
    public record GetAllTrainerEmploymentsQuery : IRequest<IEnumerable<TrainerEmployment>>
    {
        public string FitnessClubId { get; init; }
        public bool IncludeInactive { get; init; }
    }

    public class GetAllTrainerEmploymentsQueryHandler : IRequestHandler<GetAllTrainerEmploymentsQuery, IEnumerable<TrainerEmployment>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;

        public GetAllTrainerEmploymentsQueryHandler(IEmploymentRepository<TrainerEmployment> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TrainerEmployment>> Handle(GetAllTrainerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false);
        }
    }
}
