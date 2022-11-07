using Carnets.Application.Gympasses.Dtos;
using Common.Models.Dtos;

namespace Carnets.Application.Entries.Dtos
{
    public class CreatedEntryDto
    {
        public string EntryId { get; set; }

        public GympassDto Gympass { get; set; }

        public DateTime CheckInTime { get; set; }

        public bool IsValid => true;

        public MemberDto User { get; set; }
    }
}
