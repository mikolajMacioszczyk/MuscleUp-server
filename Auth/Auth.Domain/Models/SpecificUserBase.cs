using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Domain.Models
{
    public abstract class SpecificUserBase
    {
        [Key]
        [StringLength(36)]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
