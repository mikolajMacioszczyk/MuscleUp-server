using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carnets.Domain.Models
{
    public class AllowedEntriesPermission
    {
        [Key]
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }

        [Range(0, int.MaxValue)]
        public int AllowedEntries { get; set; }

        [Range(0, int.MaxValue)]
        public int AllowedEntriesCooldown { get; set; }

        // TODO: Update documentation
        public CooldownType CooldownType { get; set; }
    }
}
