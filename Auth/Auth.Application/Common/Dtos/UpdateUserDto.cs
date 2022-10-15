using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Common.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string AvatarUrl { get; set; }
    }
}
