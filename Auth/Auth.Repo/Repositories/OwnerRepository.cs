using Auth.Application.Common.Interfaces;
using Auth.Application.Owners.Dtos;
using Auth.Domain.Models;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Repo.Repositories
{
    public class OwnerRepository : UserRepositoryBase<Owner>, ISpecificUserRepository<Owner, RegisterOwnerDto>
    {
        private readonly AuthDbContext _context;

        public OwnerRepository(
            ILogger<MemberRepository> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            AuthDbContext context)
            : base(logger, userManager, userStore, signInManager)
        {
            _context = context;
        }

        protected override DbSet<Owner> Users => _context.Owners;

        public async Task<Result<Owner>> Register(RegisterOwnerDto registerDto, string userId, bool preventPasswordLogin)
        {
            var userResult = await RegisterUser(registerDto, RoleType.Trainer, userId, preventPasswordLogin);
            if (userResult.IsSuccess)
            {
                var owner = new Owner()
                {
                    User = userResult.Value,
                    UserId = userResult.Value.Id,
                };

                Users.Add(owner);
                await _context.SaveChangesAsync();

                return new Result<Owner>(owner);
            }

            return new Result<Owner>(userResult.Errors);
        }

        public async Task<Result<Owner>> UpdateData(string ownerId, Owner owner)
        {
            var ownerFromDb = await GetById(ownerId);
            if (ownerFromDb is null)
            {
                return new Result<Owner>(Common.CommonConsts.NOT_FOUND);
            }

            if (ownerFromDb.User is null)
            {
                throw new UnauthorizedAccessException();
            }

            AssignBaseUserData(owner, ownerFromDb);

            await _context.SaveChangesAsync();
            return new Result<Owner>(ownerFromDb);
        }
    }
}
