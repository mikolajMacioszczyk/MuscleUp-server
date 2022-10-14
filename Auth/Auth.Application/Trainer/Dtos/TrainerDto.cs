using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Auth.Application.Trainer.Dtos
{
    public class TrainerDto : UserDto
    {
        public override RoleType Role => RoleType.Trainer;
    }
}
