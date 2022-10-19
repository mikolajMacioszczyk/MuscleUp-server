using Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface ISpecificUserRepository<TUser, TRegistrationUser>
    {
        Task<IEnumerable<TUser>> GetAll();

        Task<TUser> GetById(string userId);

        Task<IEnumerable<TUser>> GetUsersByIds(string[] userIds);

        Task<Result<TUser>> Register(TRegistrationUser registerDto);

        Task<Result<TUser>> UpdateData(string userId, TUser user);
    }
}
