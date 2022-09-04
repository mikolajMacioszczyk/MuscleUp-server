using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
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
