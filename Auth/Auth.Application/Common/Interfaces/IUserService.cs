using Auth.Application.Common.Dtos;
using Auth.Application.Common.Models;
using Auth.Domain.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequestDto request);

        Task<ApplicationUser> GetUserById(string userId);
    }
}
