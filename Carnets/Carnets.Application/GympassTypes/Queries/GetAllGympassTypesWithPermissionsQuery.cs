using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.GympassTypes.Queries
{
    public record GetAllGympassTypesWithPermissionsQuery : IRequest<IEnumerable<GympassTypeWithPermissions>>
    {
        public string FitnessClubId { get; set; }
        public bool OnlyActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllGympassTypesWithPermissionsQueryyHandler : IRequestHandler<GetAllGympassTypesWithPermissionsQuery, IEnumerable<GympassTypeWithPermissions>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly ISender _mediator;

        public GetAllGympassTypesWithPermissionsQueryyHandler(IGympassTypeRepository gympassTypeRepository, ISender mediator)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _mediator = mediator;
        }

        public async Task<IEnumerable<GympassTypeWithPermissions>> Handle(GetAllGympassTypesWithPermissionsQuery request, CancellationToken cancellationToken)
        {
            var all = await _gympassTypeRepository.GetAllGympassTypes(
                request.FitnessClubId,
                request.OnlyActive, 
                request.PageNumber,
                request.PageSize, 
                asTracking: false);

            var allWithPermissions = new List<GympassTypeWithPermissions>();

            foreach (var gympassType in all)
            {
                var query = new AssignPermissionsToGympassTypeQuery()
                {
                    GympassType = gympassType
                };

                allWithPermissions.Add(await _mediator.Send(query));
            }

            return allWithPermissions;
        }
    }
}