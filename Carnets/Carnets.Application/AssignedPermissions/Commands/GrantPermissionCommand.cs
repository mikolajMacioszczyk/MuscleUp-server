using AutoMapper;
using Carnets.Application.AssignedPermissions.Dtos;
using Carnets.Application.Interfaces;
using Carnets.Application.Permissions.Queries;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.AssignedPermissions.Commands
{
    public record GrantPermissionCommand : IRequest<Result<AssignedPermission>>
    {
        public GrantRevokePermissionDto Model { get; init; }
        public string FitnessClubId { get; init; }
    }

    // Permission may be granted also to inactive gympass types
    public class GrantPermissionCommandHandler : IRequestHandler<GrantPermissionCommand, Result<AssignedPermission>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public GrantPermissionCommandHandler(
            IGympassTypeRepository gympassTypeRepository,
            IAssignedPermissionRepository assignedPermissionRepository,
            ISender mediator,
            IMapper mapper)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Result<AssignedPermission>> Handle(GrantPermissionCommand request, CancellationToken cancellationToken)
        {
            var grantRequest = _mapper.Map<AssignedPermission>(request.Model);

            var gympassTypeFromDb = await _gympassTypeRepository.GetGympassTypeById(grantRequest.GympassTypeId, true);
            if (gympassTypeFromDb?.FitnessClubId != request.FitnessClubId)
            {
                gympassTypeFromDb = null;
            }

            var permission = await _mediator.Send(new GetPermissionByIdQuery()
            {
                PermissionId = grantRequest.PermissionId,
                FitnessClubId = request.FitnessClubId,
                asTracking = true
            });

            if (permission is null || gympassTypeFromDb is null)
            {
                return new Result<AssignedPermission>(Common.CommonConsts.NOT_FOUND);
            }

            var createResult = await _assignedPermissionRepository.CreateAssignedPermission(grantRequest);

            if (createResult.IsSuccess)
            {
                await _assignedPermissionRepository.SaveChangesAsync();
            }

            return createResult;
        }
    }
}
