using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.SpecificPermissions.Dtos
{
    public class CreateClassPermissionDto
    {
        [Required]
        [MaxLength(30)]
        public string PermissionName { get; set; }
    }
}
