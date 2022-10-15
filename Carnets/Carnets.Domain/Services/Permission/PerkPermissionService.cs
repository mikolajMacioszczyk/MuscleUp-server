using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Services.Permission;

namespace Carnets.Domain.Services
{
    public class PerkPermissionService : PermissionServiceBase<PerkPermission>, IPermissionService<PerkPermission>
    {
        public PerkPermissionService(IPermissionRepository<PerkPermission> permissionRepository,
            IAssignedPermissionRepository assignedPermissionRepository) :
            base(permissionRepository, assignedPermissionRepository)
        {
        }
    }
}
