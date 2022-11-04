﻿using Auth.Domain.Enums;
using Auth.Domain.Models;
using Common.Consts;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repo
{
    public static class AuthDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(RoleType.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(RoleType.Worker.ToString()));
                await roleManager.CreateAsync(new IdentityRole(RoleType.Member.ToString()));
                await roleManager.CreateAsync(new IdentityRole(RoleType.Trainer.ToString()));
                await roleManager.CreateAsync(new IdentityRole(RoleType.Owner.ToString()));
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                Id = SeedConsts.DefaultAdminId,
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "John",
                LastName = "Rambo",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AvatarUrl = SeedConsts.DefaultUserAvatarUrl
            };
            await SeedUser(userManager, defaultUser, SeedConsts.DefaultPassword, RoleType.Administrator);
        }

        public static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            //Seed Default Member
            var defaultMember = new ApplicationUser
            {
                Id = SeedConsts.DefaultMemberId,
                UserName = "member",
                Email = "member@gmail.com",
                FirstName = "Lewis",
                LastName = "Hamilton",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AvatarUrl = SeedConsts.DefaultUserAvatarUrl
            };

            var memberCreated = await SeedUser(userManager, defaultMember, SeedConsts.DefaultPassword, RoleType.Member);
            if (memberCreated)
            {
                var member = new Member()
                {
                    UserId = SeedConsts.DefaultMemberId,
                    User = defaultMember,
                    HeightInCm = 180,
                    WeightInKg = 70
                };
                await context.Members.AddAsync(member);
            }

            //Seed Default Worker
            var defaultWorker = new ApplicationUser
            {
                Id = SeedConsts.DefaultWorkerId,
                UserName = "worker",
                Email = "worker@gmail.com",
                FirstName = "Charles",
                LastName = "Leclerc",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AvatarUrl = SeedConsts.DefaultUserAvatarUrl
            };

            var workerCreated = await SeedUser(userManager, defaultWorker, SeedConsts.DefaultPassword, RoleType.Worker);
            if (workerCreated)
            {
                var worker = new Worker()
                {
                    UserId = SeedConsts.DefaultWorkerId,
                    User = defaultWorker,
                };
                await context.Workers.AddAsync(worker);
            }

            //Seed Default Trainer
            var defaultTrainer = new ApplicationUser
            {
                Id = SeedConsts.DefaultTrainerId,
                UserName = "trainer",
                Email = "trainer@gmail.com",
                FirstName = "Max",
                LastName = "Verstappen",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AvatarUrl = SeedConsts.DefaultUserAvatarUrl
            };

            var trainerCreated = await SeedUser(userManager, defaultTrainer, SeedConsts.DefaultPassword, RoleType.Trainer);
            if (trainerCreated)
            {
                var trainer = new Trainer()
                {
                    UserId = SeedConsts.DefaultTrainerId,
                    User = defaultTrainer,
                };
                await context.Trainers.AddAsync(trainer);
            }

            //Seed Default Owner
            var defaultOwner = new ApplicationUser
            {
                Id = SeedConsts.DefaultOwnerId,
                UserName = "owner",
                Email = "owner@gmail.com",
                FirstName = "Sergio",
                LastName = "Perez",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                AvatarUrl = SeedConsts.DefaultUserAvatarUrl
            };

            var ownerCreated = await SeedUser(userManager, defaultOwner, SeedConsts.DefaultPassword, RoleType.Owner);
            if (ownerCreated)
            {
                var owner = new Owner()
                {
                    UserId = SeedConsts.DefaultOwnerId,
                    User = defaultOwner,
                };
                await context.Owners.AddAsync(owner);
            }

            await context.SaveChangesAsync();
        }

        private static async Task<bool> SeedUser(UserManager<ApplicationUser> userManager, ApplicationUser newUser, string password, RoleType role)
        {
            if (userManager.Users.All(u => u.Id != newUser.Id))
            {
                var user = await userManager.FindByEmailAsync(newUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(newUser, password);
                    await userManager.AddToRoleAsync(newUser, role.ToString());
                    return true;
                }
            }
            return false;
        }
    }
}
