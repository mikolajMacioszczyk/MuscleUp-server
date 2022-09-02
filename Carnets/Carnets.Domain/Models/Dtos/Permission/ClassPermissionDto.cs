namespace Carnets.Domain.Models.Dtos
{
    public class ClassPermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Enums.PermissionType.ClassPermission.ToString();

        public string PermissionName { get; set; }
    }
}
