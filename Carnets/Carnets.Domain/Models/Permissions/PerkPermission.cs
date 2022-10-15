using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class PerkPermission : PermissionBase
    {
        public override PermissionType PermissionType => PermissionType.PerkPermission;

        [Required]
        [MaxLength(30)]
        public string PermissionName { get; set; }
    }
}
