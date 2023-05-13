using Common.Exceptions;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.UserInvitations.Helpers
{
    public static class InvitationHelper
    {
        public static async Task<UserInvitation> SetInvitationUsed(
            UserInvitation invitation, 
            IUserInvitationRepository userInvitationRepository)
        {
            invitation.IsUsed = true;
            var updateResult = await userInvitationRepository.UpdateUserInvitation(invitation.InvitationId, invitation);

            if (!updateResult.IsSuccess)
            {
                throw new InvalidInputException(updateResult.ErrorCombined);
            }

            return updateResult.Value;
        }

        public static async Task<TEmployment> CreateEmploymentFromInvitation<TEmployment>(
            UserInvitation invitation, 
            string userId,
            TEmployment customEmployment,
            IEmploymentRepository<TEmployment> employmentRepository)
            where TEmployment : EmploymentBase
        {
            customEmployment.FitnessClub = invitation.FitnessClub;
            customEmployment.FitnessClubId = invitation.FitnessClub.FitnessClubId;
            customEmployment.UserId = userId;
            customEmployment.EmployedFrom = DateTime.UtcNow;
            customEmployment.EmployedTo = null;

            var employmentResult = await employmentRepository.CreateEmployment(customEmployment);

            if (!employmentResult.IsSuccess)
            {
                throw new InvalidInputException(employmentResult.ErrorCombined);
            }

            return employmentResult.Value;
        }
    }
}
