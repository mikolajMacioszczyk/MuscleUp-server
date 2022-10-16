using Auth.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime BirthDate { get; set; }

        public GenderType Gender { get; set; }

        [Required]
        [MaxLength(500)]
        public string AvatarUrl { get; set; }
    }
}
