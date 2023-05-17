using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Queries
{
    public record GetAllPermissionsQuery<TPermission> : IRequest<IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        public string FitnessClubId { get; init; }
    }

    public class GetAllPermissionsQueryHandler<TPermission> : IRequestHandler<GetAllPermissionsQuery<TPermission>, IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public GetAllPermissionsQueryHandler(IPermissionRepository<TPermission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Task<IEnumerable<TPermission>> Handle(GetAllPermissionsQuery<TPermission> request, CancellationToken cancellationToken)
        {
            return _permissionRepository.GetAll(request.FitnessClubId, false);
        }
    }
}
