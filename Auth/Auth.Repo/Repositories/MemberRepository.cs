﻿using Auth.Application.Common.Interfaces;
using Auth.Application.Members.Dtos;
using Auth.Domain.Models;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Repo.Repositories
{
    public class MemberRepository : UserRepositoryBase<Member>, ISpecificUserRepository<Member, RegisterMemberDto>
    {
        private readonly AuthDbContext _context;

        public MemberRepository(
            ILogger<MemberRepository> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager, 
            AuthDbContext context)
            : base(logger, userManager, userStore, signInManager)
        {
            _context = context;
        }

        protected override DbSet<Member> Users => _context.Members;

        public async Task<Result<Member>> Register(RegisterMemberDto registerDto, string userId, bool preventPasswordLogin)
        {
            var userResult = await RegisterUser(registerDto, RoleType.Member, userId, preventPasswordLogin);
            if (userResult.IsSuccess)
            {
                var member = new Member()
                {
                    User = userResult.Value,
                    UserId = userResult.Value.Id,
                    HeightInCm = registerDto.HeightInCm,
                    WeightInKg = registerDto.WeightInKg
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                return new Result<Member>(member);
            }

            return new Result<Member>(userResult.Errors);
        }

        public async Task<Result<Member>> UpdateData(string memberId, Member member)
        {
            var memberFromDb = await GetById(memberId);
            if (memberFromDb is null)
            {
                return new Result<Member>(Common.CommonConsts.NOT_FOUND);
            }

            if (memberFromDb.User is null)
            {
                throw new UnauthorizedAccessException();
            }

            memberFromDb.WeightInKg = member.WeightInKg;
            memberFromDb.HeightInCm = member.HeightInCm;
            AssignBaseUserData(member, memberFromDb);

            await _context.SaveChangesAsync();
            return new Result<Member>(memberFromDb);
        }
    }
}
