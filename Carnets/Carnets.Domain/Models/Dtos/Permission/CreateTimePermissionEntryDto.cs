using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateTimePermissionEntryDto
    {
        [Range(0, 24)]
        public byte EnableEntryFrom { get; set; }

        [Range(0, 24)]
        public byte EnableEntryTo { get; set; }
    }
}
