using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public abstract class PermissionBase
    {
        [Key]
        [MaxLength(36)]
        public string PermissionId { get; set; }

        public abstract PermissionType PermissionType { get; }

        [Required]
        [MaxLength(36)]
        public string FitnessClubId { get; set; }
    }
}
