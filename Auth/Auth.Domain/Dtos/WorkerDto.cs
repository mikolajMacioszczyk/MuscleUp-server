using Common.Enums;

namespace Auth.Domain.Dtos
{
    public class WorkerDto : UserDto
    {
        public override RoleType UserRole => RoleType.Worker;
    }
}
