using Auth.Application.Common.Dtos;
using Auth.Application.Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequestDto request);
    }
}
