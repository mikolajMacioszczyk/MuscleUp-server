using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClubs.Domain.Models
{
    public class WorkerEmployment
    {
        [Key]
        [MaxLength(36)]
        public string WorkerEmploymentId { get; set; }

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
