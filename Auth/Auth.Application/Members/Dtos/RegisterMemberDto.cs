using System.ComponentModel.DataAnnotations;
using Auth.Application.Common.Dtos;

namespace Auth.Application.Members.Dtos
{
    public class RegisterMemberDto : RegisterUserDto
    {
        [Range(0, double.MaxValue)]
        public double HeightInCm { get; set; }

        [Range(0, double.MaxValue)]
        public double WeightInKg { get; set; }
    }
}
