using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.Entries.Dtos
{
    public class EntryTokenDto
    {
        [Required]
        public string EntryToken { get; set; }
    }
}
