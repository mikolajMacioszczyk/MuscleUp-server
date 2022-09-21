using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Dtos
{
    public class RegisterMemberDto : RegisterUserDto
    {
        [Range(0, double.MaxValue)]
        public double Height { get; set; }

        [Range(0, double.MaxValue)]
        public double Weight { get; set; }
    }
}
