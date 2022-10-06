using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;

namespace Carnets.Domain.Services.Permission
{
    public class ClassPermissionService : PermissionServiceBase<ClassPermission>, IPermissionService<ClassPermission>
    {
        public ClassPermissionService(IPermissionRepository<ClassPermission> permissionRepository,
            IAssignedPermissionRepository assignedPermissionRepository) 
            : base(permissionRepository, assignedPermissionRepository)
        {
        }
    }
}
