using System.ComponentModel.DataAnnotations;

namespace Carnets.Application.Entries.Dtos
{
    public class GenerateEntryTokenDto
    {
        [Required]
        public string GympassId { get; set; }
    }
}
