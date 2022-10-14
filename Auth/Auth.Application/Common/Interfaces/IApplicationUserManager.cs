using Auth.Domain.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IApplicationUserManager
    {
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> GetByEmail(string email);
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task UpdateRangeAsync(IEnumerable<ApplicationUser> users);
    }
}
