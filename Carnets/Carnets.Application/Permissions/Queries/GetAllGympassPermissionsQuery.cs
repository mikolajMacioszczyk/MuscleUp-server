using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Permissions.Queries
{
    public record GetAllGympassPermissionsQuery : IRequest<Result<IEnumerable<PermissionBase>>>
    {
        public string GympassTypeId { get; set; }
    }

    public class GetAllGympassPermissionsQueryHandler : IRequestHandler<GetAllGympassPermissionsQuery, Result<IEnumerable<PermissionBase>>>
    {
        private readonly IGympassTypeRepository _gympassTypeRepository;
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public GetAllGympassPermissionsQueryHandler(IGympassTypeRepository gympassTypeRepository, IAssignedPermissionRepository assignedPermissionRepository)
        {
            _gympassTypeRepository = gympassTypeRepository;
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public async Task<Result<IEnumerable<PermissionBase>>> Handle(GetAllGympassPermissionsQuery request, CancellationToken cancellationToken)
        {
            var gympass = await _gympassTypeRepository.GetGympassTypeById(request.GympassTypeId, false);
            if (gympass is null)
            {
                return new Result<IEnumerable<PermissionBase>>(Common.CommonConsts.NOT_FOUND);
            }

            var result = await _assignedPermissionRepository.GetAllGympassPermissions(request.GympassTypeId, false);

            if (result.IsSuccess)
            {
                return new Result<IEnumerable<PermissionBase>>(result.Value.Select(a => a.Permission));
            }

            return new Result<IEnumerable<PermissionBase>>(result.ErrorCombined);
        }
    }
}
