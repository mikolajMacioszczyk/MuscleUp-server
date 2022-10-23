using Common.Models.Dtos;

namespace Carnets.Application.Interfaces
{
    public interface IMembershipService
    {
        Task CreateMembership(CreateMembershipDto membershipDto);
    }
}
