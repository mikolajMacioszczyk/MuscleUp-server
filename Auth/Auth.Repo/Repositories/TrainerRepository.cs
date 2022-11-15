using Auth.Application.Common.Interfaces;
using Auth.Application.Trainer.Dtos;
using Auth.Domain.Models;
using AutoMapper.Execution;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Repo.Repositories
{
    public class TrainerRepository : UserRepositoryBase<Trainer>, ISpecificUserRepository<Trainer, RegisterTrainerDto>
    {
        private readonly AuthDbContext _context;

        public TrainerRepository(
            ILogger<MemberRepository> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager, 
            AuthDbContext context)
            : base(logger, userManager, userStore, signInManager)
        {
            _context = context;
        }

        protected override DbSet<Trainer> Users => _context.Trainers;

        public async Task<Result<Trainer>> Register(RegisterTrainerDto registerDto, string userId, bool preventPasswordLogin)
        {
            var userResult = await RegisterUser(registerDto, RoleType.Trainer, userId, preventPasswordLogin);
            if (userResult.IsSuccess)
            {
                var trainer = new Trainer()
                {
                    User = userResult.Value,
                    UserId = userResult.Value.Id,
                };

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();

                return new Result<Trainer>(trainer);
            }

            return new Result<Trainer>(userResult.Errors);
        }

        public async Task<Result<Trainer>> UpdateData(string trainerId, Trainer trainer)
        {
            var trainerFromDb = await GetById(trainerId);
            if (trainerFromDb is null)
            {
                return new Result<Trainer>(Common.CommonConsts.NOT_FOUND);
            }

            if (trainerFromDb.User is null)
            {
                throw new UnauthorizedAccessException();
            }

            AssignBaseUserData(trainer, trainerFromDb);

            await _context.SaveChangesAsync();
            return new Result<Trainer>(trainerFromDb);
        }
    }
}
