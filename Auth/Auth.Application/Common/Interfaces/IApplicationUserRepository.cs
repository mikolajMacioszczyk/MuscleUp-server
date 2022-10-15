using Auth.Domain.Models;
using System.Linq.Expressions;

namespace Auth.Application.Common.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> GetByCondition(Expression<Func<ApplicationUser, bool>> predicate, string[] includes = null);
        Task<ApplicationUser> GetById(string id, string[] includes = null);
        Task<ApplicationUser> GetByEmail(string email);
        Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate);
        ApplicationUser Update(ApplicationUser user);
        void UpdateRange(IEnumerable<ApplicationUser> users);
        Task SaveChangesAsync();
    }
}
