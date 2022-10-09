using Common.Enums;

namespace Auth.Domain.Dtos
{
    public class TrainerDto : UserDto
    {
        public override RoleType Role => RoleType.Trainer;
    }
}
