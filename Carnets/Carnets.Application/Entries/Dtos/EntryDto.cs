using Carnets.Domain.Models;

namespace Carnets.Application.Entries.Dtos
{
    public class EntryDto
    {
        public string EntryId { get; set; }

        public Gympass Gympass { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }
    }
}
