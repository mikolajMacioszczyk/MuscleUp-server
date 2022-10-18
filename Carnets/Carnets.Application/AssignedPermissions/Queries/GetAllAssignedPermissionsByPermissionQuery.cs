using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using MediatR;

namespace Carnets.Application.AssignedPermissions.Queries
{
    public record GetAllAssignedPermissionsByPermissionQuery : IRequest<IEnumerable<AssignedPermission>>
    {
        public string PermissionId { get; init; }
    }

    public class GetAllAssignedPermissionsByPermissionQueryHandler : IRequestHandler<GetAllAssignedPermissionsByPermissionQuery, IEnumerable<AssignedPermission>>
    {
        private readonly IAssignedPermissionRepository _assignedPermissionRepository;

        public GetAllAssignedPermissionsByPermissionQueryHandler(IAssignedPermissionRepository assignedPermissionRepository)
        {
            _assignedPermissionRepository = assignedPermissionRepository;
        }

        public Task<IEnumerable<AssignedPermission>> Handle(GetAllAssignedPermissionsByPermissionQuery request, CancellationToken cancellationToken)
        {
            return _assignedPermissionRepository.GetAllByPermission(request.PermissionId, false);
        }
    }
}
