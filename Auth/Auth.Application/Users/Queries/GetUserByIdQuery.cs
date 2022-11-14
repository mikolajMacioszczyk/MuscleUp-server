using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;
using Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Queries
{
    public record GetUserByIdQuery(string UserId) : IRequest<(ApplicationUser user, RoleType userRole)> { }

    public class GetUserByIdQueryHandler : GetUserQueryBase,
        IRequestHandler<GetUserByIdQuery, (ApplicationUser, RoleType)>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetUserByIdQueryHandler(
            IApplicationUserRepository applicationUserRepository,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<(ApplicationUser, RoleType)> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepository.GetById(request.UserId);

            return await RetrieveUserRole(user);
        }
    }
}
