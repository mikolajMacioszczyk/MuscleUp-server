using Auth.Domain.Dtos;
using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequestDto request);
    }
}
