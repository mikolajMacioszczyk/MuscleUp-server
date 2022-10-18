namespace Carnets.Application.SpecificPermissions.Dtos
{
    public class ClassPermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Domain.Enums.PermissionType.ClassPermission.ToString();

        public string PermissionName { get; set; }
    }
}
