using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Common.Models.Dtos
{
    public class TrainerDto : UserDto
    {
        public override RoleType Role => RoleType.Trainer;
    }
}
