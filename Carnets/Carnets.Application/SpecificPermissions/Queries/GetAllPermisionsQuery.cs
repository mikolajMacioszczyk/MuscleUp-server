using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Queries
{
    public record GetAllPermisionsQuery<TPermission> : IRequest<IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        public string FitnessClubId { get; init; }
    }

    public class GetAllPermisionsQueryHandler<TPermission> : IRequestHandler<GetAllPermisionsQuery<TPermission>, IEnumerable<TPermission>>
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public GetAllPermisionsQueryHandler(IPermissionRepository<TPermission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Task<IEnumerable<TPermission>> Handle(GetAllPermisionsQuery<TPermission> request, CancellationToken cancellationToken)
        {
            return _permissionRepository.GetAll(request.FitnessClubId, false);
        }
    }
}
