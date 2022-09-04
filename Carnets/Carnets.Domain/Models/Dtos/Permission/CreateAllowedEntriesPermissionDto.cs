using Carnets.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateAllowedEntriesPermissionDto
    {
        [Range(0, int.MaxValue)]
        public int AllowedEntries { get; set; }

        [Range(0, int.MaxValue)]
        public int AllowedEntriesCooldown { get; set; }

        public CooldownType CooldownType { get; set; }
    }
}
