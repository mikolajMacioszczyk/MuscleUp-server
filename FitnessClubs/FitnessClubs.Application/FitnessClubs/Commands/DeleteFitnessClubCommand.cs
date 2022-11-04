using Common.Models;
using FitnessClubs.Application.Interfaces;
using MediatR;

namespace FitnessClubs.Application.FitnessClubs.Commands
{
    public record DeleteFitnessClubCommand : IRequest<Result<bool>>
    {
        public string FitnessClubId { get; init; }
    }

    public class DeleteFitnessClubCommandHandler : IRequestHandler<DeleteFitnessClubCommand, Result<bool>>
    {
        private readonly IFitnessClubRepository _repository;
        private readonly HttpAuthContext _httpAuthContext;

        public DeleteFitnessClubCommandHandler(
            IFitnessClubRepository repository, 
            HttpAuthContext httpAuthContext)
        {
            _repository = repository;
            _httpAuthContext = httpAuthContext;
        }

        public async Task<Result<bool>> Handle(DeleteFitnessClubCommand request, CancellationToken cancellationToken)
        {
            var fitnessClub = await _repository.GetById(request.FitnessClubId, false);
            if (fitnessClub is null)
            {
                return new Result<bool>(Common.CommonConsts.NOT_FOUND);
            }

            // Only fitnessClub owner or admin can delete
            if (_httpAuthContext.UserId != fitnessClub.OwnerId
                && _httpAuthContext.UserRole != Common.Enums.RoleType.Administrator)
            {
                return new Result<bool>(Common.CommonConsts.Unauthorized);
            }

            var deleteResult = await _repository.Delete(request.FitnessClubId);

            if (deleteResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return deleteResult;
        }
    }
}
