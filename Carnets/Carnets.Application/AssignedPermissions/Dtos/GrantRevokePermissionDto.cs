using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.AssignedPermissions.Dtos
{
    public class GrantRevokePermissionDto
    {
        [Required]
        [MaxLength(36)]
        public string GympassTypeId { get; set; }

        [Required]
        [MaxLength(36)]
        public string PermissionId { get; set; }
    }
}
