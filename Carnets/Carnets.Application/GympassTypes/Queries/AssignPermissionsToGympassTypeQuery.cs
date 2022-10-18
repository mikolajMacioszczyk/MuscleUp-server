using AutoMapper;
using Carnets.Application.SpecificPermissions.Queries;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.GympassTypes.Queries
{
    public record AssignPermissionsToGympassTypeQuery : IRequest<GympassTypeWithPermissions>
    {
        public GympassType GympassType { get; init; }
    }

    public class AssignPermissionsToGympassTypeQueryHandler : IRequestHandler<AssignPermissionsToGympassTypeQuery, GympassTypeWithPermissions>
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public AssignPermissionsToGympassTypeQueryHandler(IMapper mapper, ISender mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<GympassTypeWithPermissions> Handle(AssignPermissionsToGympassTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.GympassType is null)
            {
                throw new ArgumentException(nameof(request.GympassType));
            }

            var gympassWithPermissions = _mapper.Map<GympassTypeWithPermissions>(request.GympassType);

            // class
            gympassWithPermissions.ClassPermissions = (await _mediator.Send(new GetAllGympassTypePermissionsQuery<ClassPermission>()
            {
                GympassTypeId = request.GympassType.GympassTypeId
            })).Select(p => p.PermissionName);
            
            // perk
            gympassWithPermissions.PerkPermissions = (await _mediator.Send(new GetAllGympassTypePermissionsQuery<PerkPermission>()
            {
                GympassTypeId = request.GympassType.GympassTypeId
            })).Select(p => p.PermissionName);

            return gympassWithPermissions;
        }
    }
}
