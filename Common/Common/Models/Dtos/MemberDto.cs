using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Common.Models.Dtos
{
    public class MemberDto : UserDto
    {
        public double HeightInCm { get; set; }
        public double WeightInKg { get; set; }
        public override RoleType Role => RoleType.Member;
    }
}
