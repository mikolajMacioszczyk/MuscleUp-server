using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;

namespace Carnets.Domain.Services.Permission
{
    public class TimeEntryPermissionService : PermissionServiceBase<TimePermissionEntry>, IPermissionService<TimePermissionEntry>
    {
        public TimeEntryPermissionService(IPermissionRepository<TimePermissionEntry> permissionRepository,
            IAssignedPermissionRepository assignedPermissionRepository) 
            : base(permissionRepository, assignedPermissionRepository)
        {
        }
    }
}
