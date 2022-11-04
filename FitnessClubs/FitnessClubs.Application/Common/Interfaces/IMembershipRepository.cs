using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.Interfaces
{
    public interface IMembershipRepository
    {
        Task<Membership> GetMembershipById(string memberId, string fitnessClubId, bool asTracking);

        Task<IEnumerable<Membership>> GetAllMembershipsFromFitnessClub(string fitnessClubId, bool asTracking);
        
        Task<IEnumerable<Membership>> GetAllMembershipsByMember(string memberId, bool asTracking);

        Task<Membership> CreateMembership(Membership membership);

        Task SaveChangesAsync();
    }
}
