namespace Carnets.Domain.Models
{
    public class GympassTypeWithPermissions : GympassType
    {
        public IEnumerable<string> ClassPermissions { get; set; }

        public IEnumerable<string> PerkPermissions { get; set; }
    }
}
