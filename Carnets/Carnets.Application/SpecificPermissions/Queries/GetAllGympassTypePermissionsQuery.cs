using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Exceptions;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Queries
{
    public record GetAllGympassTypePermissionsQuery<TPermission> : IRequest<IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        public string GympassTypeId { get; init; }
    }

    public class GetAllGympassTypePermissionsQueryHandler<TPermission> : IRequestHandler<GetAllGympassTypePermissionsQuery<TPermission>, IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public GetAllGympassTypePermissionsQueryHandler(
            IAssignedPermissionRepository assignedPermissionRepository, 
            IPermissionRepository<TPermission> permissionRepository)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<TPermission>> Handle(GetAllGympassTypePermissionsQuery<TPermission> request, CancellationToken cancellationToken)
        {
            var allResult = await _assignedPermissionRepository.GetAllGympassPermissions(request.GympassTypeId, false);

            if (allResult.IsSuccess)
            {
                var allIds = allResult.Value.Select(a => a.PermissionId).ToArray();

                return await _permissionRepository.GetPermissionByIds(allIds, false);
            }

            throw new BadRequestException(allResult.ErrorCombined);
        }
    }
}
