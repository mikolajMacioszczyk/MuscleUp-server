using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Auth.Application.Workers.Dtos
{
    public class WorkerDto : UserDto
    {
        public override RoleType Role => RoleType.Worker;
    }
}
