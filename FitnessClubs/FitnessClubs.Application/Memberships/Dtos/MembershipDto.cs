using Common.Models.Dtos;

namespace FitnessClubs.Application.Memberships.Dtos
{
    public class MembershipDto
    {
        public string MemberId { get; set; }

        public MemberDto MemberData { get; set; }
        
        public string FitnessClubId { get; set; }

        public DateTime JoiningDate { get; set; }
    }
}
