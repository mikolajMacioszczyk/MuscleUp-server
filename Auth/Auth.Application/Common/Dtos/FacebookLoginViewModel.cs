using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Dtos
{
    public class FacebookLoginViewModel
    {
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string AvatarUrl { get; set; }
    }
}
