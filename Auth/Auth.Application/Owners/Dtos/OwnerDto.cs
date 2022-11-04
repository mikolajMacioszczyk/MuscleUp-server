using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Auth.Application.Owners.Dtos
{
    public class OwnerDto : UserDto
    {
        public override RoleType Role => RoleType.Owner;
    }
}
