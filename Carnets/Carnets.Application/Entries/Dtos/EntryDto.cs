namespace Carnets.Application.Entries.Dtos
{
    public class EntryDto
    {
        public string EntryId { get; set; }

        public string GympassId { get; set; }

        public string GympassTypeName { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }
    }
}
