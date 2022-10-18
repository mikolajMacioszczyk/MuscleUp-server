using Carnets.Domain.Enums;

namespace Carnets.Application.Dtos.Permission
{
    public class PermissionBaseDto
    {
        public string PermissionId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
