namespace Carnets.Domain.Models.Dtos
{
    public class UpdateGympassTypeWithPermissionsDto : UpdateGympassTypeDto
    {
        public IEnumerable<string> ClassPermissions { get; set; }

        public IEnumerable<string> PerkPermissions { get; set; }
    }
}
