using AutoMapper;
using Carnets.Application.Permissions.Commands;
using Carnets.Domain.Models;
using Common.Exceptions;
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

        // TODO: Replace with FluentValidation
        public static void ValidateGympassIntervals(GympassType gympassType)
        {
            if (gympassType.Interval == Domain.Enums.IntervalType.Day
                && gympassType.IntervalCount > 365)
            {
                throw new InvalidInputException("Interval count for daily subscription must be less than 365");
            };

            if (gympassType.Interval == Domain.Enums.IntervalType.Week
                && gympassType.IntervalCount > 52)
            {
                throw new InvalidInputException("Interval count for weekly subscription must be less than 53");
            };

            if (gympassType.Interval == Domain.Enums.IntervalType.Month
                && gympassType.IntervalCount > 12)
            {
                throw new InvalidInputException("Interval count for monthly subscription must be less than 13");
            };

            if (gympassType.Interval == Domain.Enums.IntervalType.Year
                && gympassType.IntervalCount > 1)
            {
                throw new InvalidInputException("Interval count for yearly subscription must be 1");
            };
        }
    }
}
