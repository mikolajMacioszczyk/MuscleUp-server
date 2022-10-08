using Common.Enums;

namespace Auth.Domain.Dtos
{
    public class MemberDto : UserDto
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public override RoleType Role => RoleType.Member;
    }
}
