using Carnets.Application.Interfaces;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Carnets.Application.FitnessClubs.Queries
{
    public record EnsureFitnessClubExistsQuery : IRequest<FitnessClubDto>
    {
        public string FitnessClubId { get; init; }
    }

    public class EnsureFitnessClubExistsQueryHandler : IRequestHandler<EnsureFitnessClubExistsQuery, FitnessClubDto>
    {
        private readonly IFitnessClubHttpService _fitnessClubHttpService;

        public EnsureFitnessClubExistsQueryHandler(IFitnessClubHttpService fitnessClubHttpService)
        {
            _fitnessClubHttpService = fitnessClubHttpService;
        }

        public async Task<FitnessClubDto> Handle(EnsureFitnessClubExistsQuery request, CancellationToken cancellationToken)
        {
            var fitnessClubResult = await _fitnessClubHttpService.GetFitnessClubById(request.FitnessClubId);
            if (!fitnessClubResult.IsSuccess)
            {
                throw new BadRequestException($"Fitness club with id {request.FitnessClubId} does not exists");
            }
            return fitnessClubResult.Value;
        }
    }
}
