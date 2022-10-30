namespace Carnets.Application.Entries.Dtos
{
    public class FailedEntryDto
    {
        public bool IsValid => false;

        public string Error { get; set; }
    }
}
