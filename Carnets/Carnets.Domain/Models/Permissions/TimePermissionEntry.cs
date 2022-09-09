using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class TimePermissionEntry : PermissionBase
    {
        public override PermissionType PermissionType => PermissionType.TimePermissionEntry;

        [Range(0, 24)]
        public byte EnableEntryFrom { get; set; }

        [Range(0, 24)]
        public byte EnableEntryTo { get; set; }
    }
}
