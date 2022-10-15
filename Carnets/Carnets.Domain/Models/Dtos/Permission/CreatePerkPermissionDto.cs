using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreatePerkPermissionDto
    {
        [Required]
        [MaxLength(30)]
        public string PerkName { get; set; }
    }
}
