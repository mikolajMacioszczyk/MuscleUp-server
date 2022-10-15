using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Common.Dtos
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistant { get; set; }
    }
}
