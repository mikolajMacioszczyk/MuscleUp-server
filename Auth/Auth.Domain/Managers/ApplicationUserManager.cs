using Auth.Domain.Interfaces;
using Auth.Domain.Models;

namespace Auth.Domain.Managers
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserManager(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public Task<ApplicationUser> GetByEmail(string email)
        {
            return _applicationUserRepository.GetByEmail(email);
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await _applicationUserRepository.GetById(id);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            user = _applicationUserRepository.Update(user);
            await _applicationUserRepository.SaveChangesAsync();

            return user;
        }

        public async Task UpdateRangeAsync(IEnumerable<ApplicationUser> users)
        {
            _applicationUserRepository.UpdateRange(users);
            await _applicationUserRepository.SaveChangesAsync();
        }
    }
}
