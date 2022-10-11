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

        public DeleteFitnessClubCommandHandler(IFitnessClubRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(DeleteFitnessClubCommand request, CancellationToken cancellationToken)
        {
            var deleteResult = await _repository.Delete(request.FitnessClubId);

            if (deleteResult.IsSuccess)
            {
                await _repository.SaveChangesAsync();
            }

            return deleteResult;
        }
    }
}
