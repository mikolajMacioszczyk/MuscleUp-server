using Auth.Domain.Enums;
using Auth.Domain.Models;
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
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                Id = "56c3389e-8fea-453e-acc2-e53a2e2768b7",
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "John",
                LastName = "Rambo",
                BirthDate = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            await SeedUser(defaultUser, "123Pa$$word.", RoleType.Administrator);

            async Task<bool> SeedUser(ApplicationUser newUser, string password, RoleType role)
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
}
