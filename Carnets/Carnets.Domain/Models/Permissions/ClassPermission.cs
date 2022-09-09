using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class ClassPermission : PermissionBase
    {
        public override PermissionType PermissionType => PermissionType.ClassPermission;

        [Required]
        [MaxLength(30)]
        public string PermissionName { get; set; }
    }
}
