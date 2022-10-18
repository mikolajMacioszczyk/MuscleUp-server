using AutoMapper;
using Carnets.Application.GympassTypes.Dtos;
using Carnets.Application.Interfaces;
using MediatR;

namespace Carnets.Application.GympassTypes.Queries
{
    public record GetGympassTypeWithPermissionsByIdQuery : IRequest<GympassTypeDto> 
    {
        public string GympassTypeId { get; init; }
    }

    public class GetGympassTypeWithPermissionsByIdQueryHandler : IRequestHandler<GetGympassTypeWithPermissionsByIdQuery, GympassTypeDto>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public GetGympassTypeWithPermissionsByIdQueryHandler(
            IGympassTypeRepository gympassTypeRepository,
            IMapper mapper,
            ISender mediator)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<GympassTypeDto> Handle(GetGympassTypeWithPermissionsByIdQuery request, CancellationToken cancellationToken)
        {
            var gympassType = await _gympassTypeRepository.GetGympassTypeById(request.GympassTypeId, false);
            if (gympassType is null)
            {
                return null;
            }

            var query = new AssignPermissionsToGympassTypeQuery()
            {
                GympassType = gympassType
            };

            var gympassWithPermissions = await _mediator.Send(query);
            return _mapper.Map<GympassTypeDto>(gympassWithPermissions);
        }
    }
}
