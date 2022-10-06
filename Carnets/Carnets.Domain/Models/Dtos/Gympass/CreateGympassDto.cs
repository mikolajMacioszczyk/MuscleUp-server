using System.ComponentModel.DataAnnotations;

namespace Carnets.Domain.Models.Dtos
{
    public class CreateGympassDto
    {
        [Required]
        [MaxLength(36)]
        public string GympassTypeId { get; set; }
    }
}
