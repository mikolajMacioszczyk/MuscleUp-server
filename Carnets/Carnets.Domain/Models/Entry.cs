using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models
{
    public class Entry
    {
        [Key]
        [MaxLength(36)]
        public string EntryId { get; set; }

        [Required]
        public Gympass Gympass { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public bool Entered { get; set; }

        public DateTime EntryExpirationTime { get; set; }
    }
}
