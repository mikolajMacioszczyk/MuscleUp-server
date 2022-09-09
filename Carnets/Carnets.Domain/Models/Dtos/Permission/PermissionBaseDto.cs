using Carnets.Domain.Enums;

namespace Carnets.Domain.Models.Dtos
{
    public class PermissionBaseDto
    {
        public string PermissionId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
