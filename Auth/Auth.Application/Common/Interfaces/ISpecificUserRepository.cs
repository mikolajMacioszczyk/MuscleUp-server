using Auth.Domain.Models;
using Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface ISpecificUserRepository<TUser, TRegistrationUser>
    {
        Task<IEnumerable<TUser>> GetAll();

        Task<TUser> GetById(string memberId);

        Task<Result<TUser>> Register(TRegistrationUser registerDto);

        Task<Result<TUser>> UpdateData(string memberId, TUser member);
    }
}
