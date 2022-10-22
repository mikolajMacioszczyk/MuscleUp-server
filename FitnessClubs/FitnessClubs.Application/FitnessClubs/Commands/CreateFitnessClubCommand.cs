using Common.Consts;
using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Commands
{
    public record CreateFitnessClubCommand : IRequest<Result<FitnessClub>>
    {
        public FitnessClub FitnessClub { get; init; }
    }

    public class CreateFitnessClubCommandHandler : IRequestHandler<CreateFitnessClubCommand, Result<FitnessClub>>
    {
        private readonly IFitnessClubRepository _repository;

        public CreateFitnessClubCommandHandler(IFitnessClubRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<FitnessClub>> Handle(CreateFitnessClubCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FitnessClub.FitnessClubLogoUrl))
            {
                request.FitnessClub.FitnessClubLogoUrl = SeedConsts.DefaultFitnessClubLogoUrl;
            }

            var createResult = await _repository.Create(request.FitnessClub);

            if (createResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
