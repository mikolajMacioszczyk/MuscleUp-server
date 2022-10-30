using Carnets.Domain.Models;
using Common.Models.Dtos;

namespace Carnets.Application.Entries.Dtos
{
    public class CreatedEntryDto
    {
        public string EntryId { get; set; }

        public Gympass Gympass { get; set; }

        public DateTime CheckInTime { get; set; }

        public bool IsValid => true;

        public MemberDto User { get; set; }
    }
}
