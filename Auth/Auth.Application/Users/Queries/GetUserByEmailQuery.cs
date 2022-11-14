using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;
using Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<(ApplicationUser user, RoleType userRole)> { }

    public class GetUserByEmailQueryHandler : GetUserQueryBase,
        IRequestHandler<GetUserByEmailQuery, (ApplicationUser, RoleType)>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetUserByEmailQueryHandler(
            IApplicationUserRepository applicationUserRepository, 
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<(ApplicationUser, RoleType)> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepository.GetByEmail(request.Email);

            return await RetrieveUserRole(user);
        }
    }
}
