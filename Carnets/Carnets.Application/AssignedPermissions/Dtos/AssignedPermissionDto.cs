using Carnets.Domain.Models;

namespace Carnets.Application.AssignedPermissions.Dtos
{
    public class AssignedPermissionDto
    {
        public GympassType GympassType { get; set; }

        public PermissionBase Permission { get; set; }
    }
}
