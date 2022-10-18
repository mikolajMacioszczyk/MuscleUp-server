namespace Carnets.Application.SpecificPermissions.Dtos
{
    public class PerkPermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Domain.Enums.PermissionType.PerkPermission.ToString();

        public string PermissionName { get; set; }
    }
}
