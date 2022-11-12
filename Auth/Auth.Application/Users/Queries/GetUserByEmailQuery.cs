using Auth.Application.Common.Interfaces;
using Auth.Domain.Models;
using MediatR;

namespace Auth.Application.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<ApplicationUser> { }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApplicationUser>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetUserByEmailQueryHandler(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public Task<ApplicationUser> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return _applicationUserRepository.GetByEmail(request.Email);
        }
    }
}
