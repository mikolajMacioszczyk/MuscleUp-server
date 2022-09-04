using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateClassPermissionDto
    {
        [Required]
        [MaxLength(30)]
        public string PermissionName { get; set; }
    }
}
