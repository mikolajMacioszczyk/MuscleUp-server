using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carnets.Domain.Models
{
    public class AssignedPermission
    {
        // TODO: Temporary. Combained key with Permission
        [Key]
        [ForeignKey("GympassType")]
        public string GympassTypeId { get; set; }
        public GympassType GympassType { get; set; }

        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
