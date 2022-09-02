using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Permission
    {
        [Key]
        [MaxLength(36)]
        public string PermissionId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
