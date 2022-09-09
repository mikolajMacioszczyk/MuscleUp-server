namespace Carnets.Domain.Models.Dtos
{
    public class TimePermissionEntryDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Enums.PermissionType.TimePermissionEntry.ToString();

        public byte EnableEntryFrom { get; set; }

        public byte EnableEntryTo { get; set; }
    }
}
