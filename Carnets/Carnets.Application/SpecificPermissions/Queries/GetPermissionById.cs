using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.SpecificPermissions.Queries
{
    public record GetPermissionById<TPermission> : IRequest<TPermission>
        where TPermission : PermissionBase
    {
        public string PermissionId { get; init; }
    }

    public class GetPermissionByIdHandler<TPermission> : IRequestHandler<GetPermissionById<TPermission>, TPermission>
        where TPermission : PermissionBase
    {
        private readonly IPermissionRepository<TPermission> _permissionRepository;

        public GetPermissionByIdHandler(IPermissionRepository<TPermission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Task<TPermission> Handle(GetPermissionById<TPermission> request, CancellationToken cancellationToken)
        {
            return _permissionRepository.GetPermissionById(request.PermissionId, false);
        }
    }
}
