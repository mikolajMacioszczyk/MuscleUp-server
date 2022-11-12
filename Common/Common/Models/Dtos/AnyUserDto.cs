using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Common.Models.Dtos
{
    public class AnyUserDto : UserDto
    {
        public override RoleType Role => RoleType.None;
    }
}
