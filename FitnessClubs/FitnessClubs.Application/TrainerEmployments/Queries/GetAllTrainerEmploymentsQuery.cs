using Common.Exceptions;
using Common.Models.Dtos;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Queries
{
    public record GetAllTrainerEmploymentsQuery : IRequest<IEnumerable<TrainerDto>>
    {
        public string FitnessClubId { get; init; }
        public bool IncludeInactive { get; init; }
    }

    public class GetAllTrainerEmploymentsQueryHandler : IRequestHandler<GetAllTrainerEmploymentsQuery, IEnumerable<TrainerDto>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;
        private readonly IAuthService _authService;

        public GetAllTrainerEmploymentsQueryHandler(
            IEmploymentRepository<TrainerEmployment> repository, 
            IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetAllTrainerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            var employments = await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false);

            var userIds = employments.Select(e => e.UserId);

            var trainersResult = await _authService.GetAllTrainersWithIds(userIds);

            if (trainersResult.IsSuccess)
            {
                return trainersResult.Value;
            }

            throw new BadRequestException(trainersResult.ErrorCombined);
        }
    }
}
