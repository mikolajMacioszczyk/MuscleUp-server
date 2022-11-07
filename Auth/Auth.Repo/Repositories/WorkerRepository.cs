using Auth.Application.Common.Interfaces;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Repo.Repositories
{
    public class WorkerRepository : UserRepositoryBase<Worker>, ISpecificUserRepository<Worker, RegisterWorkerDto>
    {
        private readonly AuthDbContext _context;
        public WorkerRepository(
            ILogger<MemberRepository> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager, 
            AuthDbContext context)
            : base(logger, userManager, userStore, signInManager)
        {
            _context = context;
        }

        protected override DbSet<Worker> Users => _context.Workers;

        public async Task<Result<Worker>> Register(RegisterWorkerDto registerDto, string userId, bool preventPasswordLogin)
        {
            var userResult = await RegisterUser(registerDto, RoleType.Worker, userId, preventPasswordLogin);
            if (userResult.IsSuccess)
            {
                var worker = new Worker()
                {
                    User = userResult.Value,
                    UserId = userResult.Value.Id,
                };

                _context.Workers.Add(worker);
                await _context.SaveChangesAsync();

                return new Result<Worker>(worker);
            }

            return new Result<Worker>(userResult.Errors);
        }

        public async Task<Result<Worker>> UpdateData(string workerId, Worker worker)
        {
            var workerFromDb = await GetById(workerId);
            if (workerFromDb is null)
            {
                return new Result<Worker>(Common.CommonConsts.NOT_FOUND);
            }

            if (workerFromDb.User is null)
            {
                throw new UnauthorizedAccessException();
            }

            workerFromDb.User.FirstName = worker.User.FirstName;
            workerFromDb.User.LastName = worker.User.LastName;
            workerFromDb.User.BirthDate = worker.User.BirthDate;
            workerFromDb.User.AvatarUrl = worker.User.AvatarUrl;

            await _context.SaveChangesAsync();
            return new Result<Worker>(workerFromDb);
        }
    }
}
