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
        private readonly HttpAuthContext _httpAuthContext;

        public CreateFitnessClubCommandHandler(
            IFitnessClubRepository repository, 
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<Result<FitnessClub>> Handle(CreateFitnessClubCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FitnessClub.FitnessClubLogoUrl))
            {
                request.FitnessClub.FitnessClubLogoUrl = SeedConsts.DefaultFitnessClubLogoUrl;
            }

            if (_httpAuthContext.UserRole == Common.Enums.RoleType.Administrator)
            {
                // TODO: Validate owner exists
            }
            else if (_httpAuthContext.UserRole == Common.Enums.RoleType.Owner)
            {
                // assign FitnessClub owner
                request.FitnessClub.OwnerId = _httpAuthContext.UserId;
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
