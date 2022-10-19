using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessClubs.Domain.Models
{
    public abstract class EmploymentBase
    {
        [Key]
        [MaxLength(36)]
        public string EmploymentId { get; set; }

        [MaxLength(36)]
        public string UserId { get; set; }

        [MaxLength(36)]
        [ForeignKey(nameof(FitnessClub))]
        public string FitnessClubId { get; set; }

        public FitnessClub FitnessClub { get; set; }

        public DateTime EmployedFrom { get; set; }

        public DateTime? EmployedTo { get; set; }

        public bool IsActive => !EmployedTo.HasValue;
    }
}
