using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;

namespace Carnets.Domain.Services.Permission
{

    public class AllowedEntriesPermissionService : PermissionServiceBase<AllowedEntriesPermission>, IPermissionService<AllowedEntriesPermission>
    {
        public AllowedEntriesPermissionService(IPermissionRepository<AllowedEntriesPermission> permissionRepository,
            IAssignedPermissionRepository assignedPermissionRepository) : 
            base(permissionRepository, assignedPermissionRepository)
        {
        }
    }
}
