using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.TrainerEmployments.Dtos;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Queries
{
    public record GetAllTrainerEmploymentsQuery(string FitnessClubId, bool IncludeInactive) 
        : IRequest<IEnumerable<TrainerEmploymentWithUserDataDto>>
    { }

    public class GetAllTrainerEmploymentsQueryHandler : IRequestHandler<GetAllTrainerEmploymentsQuery, IEnumerable<TrainerEmploymentWithUserDataDto>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _repository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetAllTrainerEmploymentsQueryHandler(
            IEmploymentRepository<TrainerEmployment> repository,
            IAuthService authService,
            IMapper mapper)
        {
            _repository = repository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerEmploymentWithUserDataDto>> Handle(GetAllTrainerEmploymentsQuery request, CancellationToken cancellationToken)
        {
            var employments = _mapper.Map<IEnumerable<TrainerEmploymentWithUserDataDto>>(
                await _repository.GetAllEmployments(request.FitnessClubId, request.IncludeInactive, false));

            var userIds = employments.Select(e => e.UserId);

            var trainersResult = await _authService.GetAllTrainersWithIds(userIds);

            if (trainersResult.IsSuccess)
            {
                var trainersData = trainersResult.Value.ToList();

                // assign trainer data
                foreach (var employment in employments)
                {
                    employment.TrainerData = trainersData.FirstOrDefault(w => w.UserId == employment.UserId);
                }

                return employments;
            }

            throw new BadRequestException(trainersResult.ErrorCombined);
        }
    }
}
