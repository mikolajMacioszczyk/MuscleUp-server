using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Repo.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly HttpAuthContext _httpAuthContext;
        protected AuthDbContext _context;

        public ApplicationUserRepository(AuthDbContext context, HttpAuthContext httpAuthContext)
        {
            _httpAuthContext = httpAuthContext;
            _context = context;
        }

        public async Task<ApplicationUser> GetById(string id, string[] includes = null)
        {
            var query = _context.Users
                .Where(u => u.Id == id);
            if (includes != null && includes.Any())
            {
                query = includes!.Aggregate(query, (current, item) => current.Include(item));
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return _context.Users.AnyAsync(predicate);
        }

        public async Task<ApplicationUser> GetByCondition(Expression<Func<ApplicationUser, bool>> predicate, string[] includes = null)
        {
            var query = _context.Users.Where(predicate);

            if (includes != null && includes.Any())
            {
                query = includes!.Aggregate(query, (current, item) => current.Include(item));
            }

            return await query.FirstOrDefaultAsync();
        }

        public ApplicationUser Update(ApplicationUser user)
        {
            _context.Attach(user);

            return _context.Users.Update(user).Entity;
        }

        public void UpdateRange(IEnumerable<ApplicationUser> users)
        {
            _context.Users.UpdateRange(users);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
