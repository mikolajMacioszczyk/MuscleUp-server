using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.SpecificPermissions.Dtos
{
    public class CreatePerkPermissionDto
    {
        [Required]
        [MaxLength(30)]
        public string PermissionName { get; set; }
    }
}
