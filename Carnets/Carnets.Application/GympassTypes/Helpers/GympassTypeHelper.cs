using AutoMapper;
using Carnets.Application.Permissions.Commands;
using Carnets.Domain.Models;
using Common.Models;
using MediatR;

namespace Carnets.Application.Helpers
{
    public static class GympassTypeHelper
    {
        public static GympassTypeWithPermissions MapToGympassTypeWithPermissions(GympassType source,
            IEnumerable<string> classPermissionNames,
            IEnumerable<string> perkPermissionNames,
            IMapper mapper)
        {
            var gympassWithPermissions = mapper.Map<GympassTypeWithPermissions>(source);
            gympassWithPermissions.ClassPermissions = classPermissionNames;
            gympassWithPermissions.PerkPermissions = perkPermissionNames;

            return gympassWithPermissions;
        }

        public static async Task<Result<GympassType>> AssignAllGympassPermissions(
           IEnumerable<ClassPermission> classPermissions,
           IEnumerable<PerkPermission> perkPermissions,
           GympassType createdGympassType,
           ISender _mediator)
        {

            var assignResult = await _mediator.Send(new AssignGympassPermissionsCommand()
            {
                GympassType = createdGympassType,
                Permissions = classPermissions
            });

            if (!assignResult.IsSuccess)
            {
                return assignResult;
            }

            assignResult = await _mediator.Send(new AssignGympassPermissionsCommand()
            {
                GympassType = createdGympassType,
                Permissions = perkPermissions
            });
            if (!assignResult.IsSuccess)
            {
                return assignResult;
            }

            return new Result<GympassType>(createdGympassType);
        }
    }
}
