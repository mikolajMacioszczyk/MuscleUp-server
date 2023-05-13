using Common.Exceptions;
using Common.Interfaces;
using Common.Models.Dtos;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Queries
{
    public record GetAllEmployedWorkersQuery(string FitnessClubId, bool IncludeInactive) : IRequest<IEnumerable<WorkerDto>> 
    {}

    public class GetAllEmployedWorkersQueryHandler : IRequestHandler<GetAllEmployedWorkersQuery, IEnumerable<WorkerDto>>
    {
        private readonly IEmploymentRepository<WorkerEmployment> _repository;
        private readonly IAuthService _authService;

        public GetAllEmployedWorkersQueryHandler(
            IEmploymentRepository<WorkerEmployment> repository, 
            IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IEnumerable<WorkerDto>> Handle(GetAllEmployedWorkersQuery request, CancellationToken cancellationToken)
        {
            var employments = await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false);

            var userIds = employments.Select(e => e.UserId);

            var workersResult = await _authService.GetAllWorkersWithIds(userIds);

            if (workersResult.IsSuccess)
            {
                return workersResult.Value;
            }

            throw new InvalidInputException(workersResult.ErrorCombined);
        }
    }
}
