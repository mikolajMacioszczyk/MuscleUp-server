using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.WorkoutEmployments.Dtos;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.WorkoutEmployments.Queries
{
    public record GetAllWorkerEmploymentsQuery(string FitnessClubId, bool IncludeInactive) 
        : IRequest<IEnumerable<WorkerEmploymentWithUserDataDto>>
    { }

    public class GetAllWorkerEmploymentsQueryHandler : IRequestHandler<GetAllWorkerEmploymentsQuery, IEnumerable<WorkerEmploymentWithUserDataDto>>
    {
        private readonly IEmploymentRepository<WorkerEmployment> _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetAllWorkerEmploymentsQueryHandler(
            IEmploymentRepository<WorkerEmployment> repository,
            IAuthService authService,
            IMapper mapper)
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerEmploymentWithUserDataDto>> Handle(GetAllWorkerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            var employments = _mapper.Map<IEnumerable<WorkerEmploymentWithUserDataDto>>(
                await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false));

            var userIds = employments.Select(e => e.UserId);

            var workersResult = await _authService.GetAllWorkersWithIds(userIds);

            if (workersResult.IsSuccess)
            {
                var workersData = workersResult.Value.ToList();

                // assign worker data
                foreach (var employment in employments)
                {
                    employment.UserData = workersData.FirstOrDefault(w => w.UserId == employment.UserId);
                }

                return employments;
            }

            throw new BadRequestException(workersResult.ErrorCombined);
        }
    }
}
