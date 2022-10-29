﻿using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Consts;
using Microsoft.EntityFrameworkCore;

namespace Carnets.Repo
{
    public static class CarnetsDbContextSeed
    {
        public static async Task SeedDefaultCarnetsDataAsync(CarnetsDbContext context, IPaymentService _paymentService)
        {
            if (await context.GympassTypes.AnyAsync())
            {
                return;
            }

            // seed gympass type data
            var defaultGympassType = new GympassType()
            {
                GympassTypeId = SeedConsts.DefaultGympassTypeId,
                GympassTypeName = "Default Gympass Type",
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                Price = 100,
                IsActive = true,
                EnableEntryFromInMinutes = 6 * 60,
                EnableEntryToInMinutes = 20 * 60,
                Interval = Domain.Enums.IntervalType.Month,
                IntervalCount = 1,
                Version = 1
            };

            await context.GympassTypes.AddAsync(defaultGympassType);

            await _paymentService.EnsureProductCreated(defaultGympassType);

            // seed gympass permission data
            var defaultClassPermission = new ClassPermission()
            {
                PermissionId = SeedConsts.DefaultClassPermissionId,
                FitnessClubId = SeedConsts.DefaultFitnessClubId,
                PermissionName = "Default Class Permission",
            };

            await context.ClassPermissions.AddAsync(defaultClassPermission);

            // seed gympass assigned permission data
            var defaultPermissionAssigement = new AssignedPermission()
            {
                GympassTypeId = SeedConsts.DefaultGympassTypeId,
                GympassType = defaultGympassType,
                PermissionId = SeedConsts.DefaultClassPermissionId,
                Permission = defaultClassPermission
            };

            await context.AssignedPermissions.AddAsync(defaultPermissionAssigement);

            // seed gympass data
            var defaultGympass = new Gympass()
            {
                GympassId = SeedConsts.DefaultGympassId,
                GympassType = defaultGympassType,
                UserId = SeedConsts.DefaultMemberId,
                Status = Domain.Enums.GympassStatus.Active,
                ValidityDate = DateTime.UtcNow.AddDays(30),
                ActivationDate = DateTime.UtcNow
            };

            await context.Gympasses.AddAsync(defaultGympass);

            // seed subscription data
            var defaultSubscription = new Subscription()
            {
                SubscriptionId = SeedConsts.DefaultSubscriptionId,
                Gympass = defaultGympass,
                StripeCustomerId = "default stripe customer id",
                StripePaymentmethodId = "subscription",
            };

            await context.Subscriptions.AddAsync(defaultSubscription);

            await context.SaveChangesAsync();
        }
    }
}
