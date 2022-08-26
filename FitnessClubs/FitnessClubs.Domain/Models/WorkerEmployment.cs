using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClubs.Domain.Models
{
    public class WorkerEmployment
    {
        [MaxLength(30)]
        public string UserId { get; set; }

        [MaxLength(30)]
        [ForeignKey(nameof(FitnessClub))]
        public string FitnessClubId { get; set; }

        public FitnessClub FitnessClub { get; set; }
    }
}
