using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carnets.Domain.Models
{
    public class TimePermissionEntry
    {
        [Key]
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }

        [Range(0, 24)]
        public byte EnableEntryFrom { get; set; }

        [Range(0, 24)]
        public byte EnableEntryTo { get; set; }
    }
}
