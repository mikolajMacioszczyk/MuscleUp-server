namespace Carnets.Domain.Models.Dtos
{
    public class PerkPermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Enums.PermissionType.PerkPermission.ToString();

        public string PermissionName { get; set; }
    }
}
