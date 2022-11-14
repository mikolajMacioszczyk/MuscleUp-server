using Auth.Domain.Models;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Queries
{
    public abstract class GetUserQueryBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        protected GetUserQueryBase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<(ApplicationUser, RoleType)> RetrieveUserRole(ApplicationUser user)
        {
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    var userRole = Enum.Parse<RoleType>(roles.FirstOrDefault());
                    return (user, userRole);
                }
            }

            return (user, RoleType.None);
        }
    }
}
