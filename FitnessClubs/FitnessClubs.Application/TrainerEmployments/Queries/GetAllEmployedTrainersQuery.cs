using Common.Exceptions;
using Common.Interfaces;
using Common.Models.Dtos;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Queries
{
    public record GetAllEmployedTrainersQuery(string FitnessClubId, bool IncludeInactive) : IRequest<IEnumerable<TrainerDto>>
    { }

    public class GetAllEmployedTrainersQueryHandler : IRequestHandler<GetAllEmployedTrainersQuery, IEnumerable<TrainerDto>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;
        private readonly IAuthService _authService;

        public GetAllEmployedTrainersQueryHandler(
            IEmploymentRepository<TrainerEmployment> repository, 
            IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetAllEmployedTrainersQuery request, CancellationToken cancellationToken)
        {
            var employments = await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false);

            var userIds = employments.Select(e => e.UserId);

            var trainersResult = await _authService.GetAllTrainersWithIds(userIds);

            if (trainersResult.IsSuccess)
            {
                return trainersResult.Value;
            }

            throw new InvalidInputException(trainersResult.ErrorCombined);
        }
    }
}
