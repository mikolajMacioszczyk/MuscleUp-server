using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carnets.Domain.Models
{
    public class AssignedPermission
    {
        [Key]
        [ForeignKey("GympassType")]
        public string GympassTypeId { get; set; }
        public GympassType GympassType { get; set; }

        [Key]
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
