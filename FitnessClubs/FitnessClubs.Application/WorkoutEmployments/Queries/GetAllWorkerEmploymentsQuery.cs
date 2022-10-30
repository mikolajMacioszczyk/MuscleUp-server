using Common.Exceptions;
using Common.Interfaces;
using Common.Models.Dtos;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Queries
{
    public record GetAllWorkerEmploymentsQuery : IRequest<IEnumerable<WorkerDto>> 
    {
        public string FitnessClubId { get; init; }
        public bool IncludeInactive { get; init; }
    }

    public class GetAllFitnessClubsQueryHandler : IRequestHandler<GetAllWorkerEmploymentsQuery, IEnumerable<WorkerDto>>
    {
        private readonly IEmploymentRepository<WorkerEmployment> _repository;
        private readonly IAuthService _authService;

        public GetAllFitnessClubsQueryHandler(
            IEmploymentRepository<WorkerEmployment> repository, 
            IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IEnumerable<WorkerDto>> Handle(GetAllWorkerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            var employments = await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false);

            var userIds = employments.Select(e => e.UserId);

            var workersResult = await _authService.GetAllWorkersWithIds(userIds);

            if (workersResult.IsSuccess)
            {
                return workersResult.Value;
            }

            throw new BadRequestException(workersResult.ErrorCombined);
        }
    }
}
