using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Auth.Application.Members.Dtos
{
    public class MemberDto : UserDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public override RoleType Role => RoleType.Member;
    }
}
