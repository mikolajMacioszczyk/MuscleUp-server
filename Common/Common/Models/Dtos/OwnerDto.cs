using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Common.Models.Dtos
{
    public class OwnerDto : UserDto
    {
        public override RoleType Role => RoleType.Owner;
    }
}
