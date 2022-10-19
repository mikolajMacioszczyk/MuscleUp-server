using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.TrainerEmployments.Commands
{
    public record CreateTrainerEmploymentCommand : IRequest<Result<TrainerEmployment>>
    {
        public TrainerEmployment TrainerEmployment { get; init; }
    }

    public class CreateTrainerEmploymentCommandHandler : IRequestHandler<CreateTrainerEmploymentCommand, Result<TrainerEmployment>>
    {
        private readonly IEmploymentRepository<TrainerEmployment> _employmentRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;

        public CreateTrainerEmploymentCommandHandler(
            IFitnessClubRepository fitnessClubRepository,
            IEmploymentRepository<TrainerEmployment> employmentRepository)
        {
            _fitnessClubRepository = fitnessClubRepository;
            _employmentRepository = employmentRepository;
        }

        public async Task<Result<TrainerEmployment>> Handle(CreateTrainerEmploymentCommand request, CancellationToken cancellationToken)
        {
            // TODO: Validate UserId
            var trainerEmployment = request.TrainerEmployment;
            var fitnessClubFromDb = await _fitnessClubRepository.GetById(trainerEmployment.FitnessClubId, true);

            if (fitnessClubFromDb is null)
            {
                return new Result<TrainerEmployment>(Common.CommonConsts.NOT_FOUND);
            }

            trainerEmployment.FitnessClub = fitnessClubFromDb;
            trainerEmployment.FitnessClubId = fitnessClubFromDb.FitnessClubId;

            var createResult = await _employmentRepository.CreateEmployment(trainerEmployment);

            if (createResult.IsSuccess)
            {
                await _employmentRepository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
