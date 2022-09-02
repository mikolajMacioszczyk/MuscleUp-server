namespace Carnets.Domain.Models.Dtos
{
    public class AllowedEntriesPermissionDto
    {
        public string PermissionId { get; set; }

        public string PermissionType => Enums.PermissionType.AllowedEntriesPermission.ToString();

        public int AllowedEntries { get; set; }

        public int AllowedEntriesCooldown { get; set; }

        public string CooldownType { get; set; }
    }
}
