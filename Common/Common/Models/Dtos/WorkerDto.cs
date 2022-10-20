using Auth.Application.Common.Dtos;
using Common.Enums;

namespace Common.Models.Dtos
{
    public class WorkerDto : UserDto
    {
        public override RoleType Role => RoleType.Worker;
    }
}
