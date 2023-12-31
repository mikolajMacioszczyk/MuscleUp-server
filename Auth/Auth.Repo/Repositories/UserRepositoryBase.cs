﻿using Auth.Application.Common.Dtos;
using Auth.Domain.Models;
using Common.Consts;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Repo.Repositories
{
    public abstract class UserRepositoryBase<TUser>
        where TUser : SpecificUserBase
    {
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly ILogger<MemberRepository> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IUserStore<ApplicationUser> _userStore;

        protected abstract DbSet<TUser> Users { get; }

        public UserRepositoryBase(
            ILogger<MemberRepository> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<TUser>> GetAll() =>
            await Users.Include(t => t.User).ToListAsync();

        public async Task<TUser> GetById(string memberId) => 
            await Users.Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == memberId);

        public async Task<IEnumerable<TUser>> GetUsersByIds(string[] userIds)
        {
            return await Users.Include(m => m.User)
                .Where(m => userIds.Contains(m.UserId))
                .ToListAsync();
        }

        protected async Task<Result<ApplicationUser>> RegisterUser(
            RegisterUserDto registerDto, 
            RoleType role, 
            string userId = null,
            bool preventPasswordLogin = true)
        {
            var userByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userByEmail != null)
            {
                return new Result<ApplicationUser>($"User with email {registerDto.Email} already exists");
            }

            var avatarUrl = string.IsNullOrEmpty(registerDto.AvatarUrl) ? SeedConsts.DefaultUserAvatarUrl : registerDto.AvatarUrl;

            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                BirthDate = registerDto.BirthDate.ToUniversalTime(),
                RegisterDate = DateTime.UtcNow,
                Gender = registerDto.Gender,
                AvatarUrl = avatarUrl,
                PreventPasswordLogin = preventPasswordLogin
            };

            if (!string.IsNullOrEmpty(userId))
            {
                user.Id = userId;
            }

            await _userStore.SetUserNameAsync(user, registerDto.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                string roleName = role.ToString();
                await _userManager.AddToRoleAsync(user, roleName);
                _logger.LogInformation($"User {roleName} created with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return new Result<ApplicationUser>(user);
            }

            return new Result<ApplicationUser>(result.Errors.Select(e => e.Description).ToArray());
        }

        protected void AssignBaseUserData(SpecificUserBase source, SpecificUserBase updated)
        {
            updated.User.FirstName = source.User.FirstName;
            updated.User.LastName = source.User.LastName;
            updated.User.BirthDate = source.User.BirthDate.ToUniversalTime();
            updated.User.AvatarUrl = source.User.AvatarUrl;
        }
    }
}
