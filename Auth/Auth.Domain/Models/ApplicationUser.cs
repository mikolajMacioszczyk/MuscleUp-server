using Auth.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderType Gender { get; set; }
    }
}
