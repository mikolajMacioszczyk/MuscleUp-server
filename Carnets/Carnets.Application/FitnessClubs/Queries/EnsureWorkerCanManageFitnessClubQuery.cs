using Carnets.Application.Interfaces;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace Carnets.Application.FitnessClubs.Queries
{
    public record EnsureWorkerCanManageFitnessClubQuery : IRequest<FitnessClubDto>
    {
        public string WorkerId { get; init; }
    }

    public class EnsureWorkerCanManageFitnessClubQueryHandler : IRequestHandler<EnsureWorkerCanManageFitnessClubQuery, FitnessClubDto>
    {
        private readonly IFitnessClubHttpService _fitnessClubHttpService;

        public EnsureWorkerCanManageFitnessClubQueryHandler(IFitnessClubHttpService fitnessClubHttpService)
        {
            _fitnessClubHttpService = fitnessClubHttpService;
        }

        public async Task<FitnessClubDto> Handle(EnsureWorkerCanManageFitnessClubQuery request, CancellationToken cancellationToken)
        {
            var fitnessClubResult = await _fitnessClubHttpService.GetFitnessClubOfWorker(request.WorkerId);
            if (!fitnessClubResult.IsSuccess)
            {
                throw new BadRequestException("Cannot determine worker's fitness club");
            }
            return fitnessClubResult.Value;
        }
    }
}
