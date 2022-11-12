using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;
using Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<(ApplicationUser user, RoleType userRole)> { }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, (ApplicationUser, RoleType)>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByEmailQueryHandler(
            IApplicationUserRepository applicationUserRepository, 
            UserManager<ApplicationUser> userManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
        }

        public async Task<(ApplicationUser, RoleType)> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepository.GetByEmail(request.Email);

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
