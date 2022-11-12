using Common.Enums;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FitnessClubs.Application.UserInvitations.Commands
{
    public record GenerateUserInvitationCommand(string FitnessClubId, string Email, string BaseInvitationLink, RoleType RoleType)
        : IRequest<Result<UserInvitation>>
    { }

    public class GenerateWorkerInvitationCommandHandler : IRequestHandler<GenerateUserInvitationCommand, Result<UserInvitation>>
    {
        private const string InvitationTokenParamName = "token";
        private const string FitnessClubParamName = "fitness-club"; 
        
        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;
        private readonly int _invitationValidityInDays;

        public GenerateWorkerInvitationCommandHandler(
            IUserInvitationRepository userInvitationRepository,
            IFitnessClubRepository fitnessClubRepository,
            IEmailService emailService,
            IConfiguration configuration,
            IAuthService authService)
        {
            _userInvitationRepository = userInvitationRepository;
            _fitnessClubRepository = fitnessClubRepository;

            _invitationValidityInDays = configuration.GetValue<int>("InvitationValidityInDays");
            _emailService = emailService;
            _authService = authService;
        }

        public async Task<Result<UserInvitation>> Handle(GenerateUserInvitationCommand request, CancellationToken cancellationToken)
        {
            var fitnessClub = await _fitnessClubRepository.GetById(request.FitnessClubId, true);

            if (fitnessClub is null)
            {
                return new Result<UserInvitation>($"Fitness club with id = {request.FitnessClubId} does not exists");
            }

            // Validate if user with provided email does not exists
            var existingUser = await _authService.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                // worker cannot have multiple employment
                if (existingUser.Role != request.RoleType || existingUser.Role == RoleType.Worker)
                {
                    return new Result<UserInvitation>($"User with email = {request.Email} already exists");
                }
            }

            var invitation = new UserInvitation()
            {
                Email = request.Email,
                FitnessClub = fitnessClub,
                IsUsed = false,
                UserType = request.RoleType,
                ExpirationDateTime = DateTime.UtcNow.AddDays(_invitationValidityInDays),
            };

            var created = await _userInvitationRepository.CreateUserInvitation(invitation);

            await _userInvitationRepository.SaveChangesAsync();

            await SendInvitationEmail(request.Email, created.InvitationId, fitnessClub, request.BaseInvitationLink, request.RoleType);

            return new Result<UserInvitation>(created);
        }

        private async Task SendInvitationEmail(
            string recipientEmailAddress, 
            string invitationId,
            FitnessClub fitnessClub,
            string baseInvitationLink,
            RoleType roleType)
        {
            var invitationRole = roleType.ToString();

            var invitationLink = baseInvitationLink
                .AppendQueryParamToUri(InvitationTokenParamName, invitationId)
                .AppendQueryParamToUri(FitnessClubParamName, fitnessClub.FitnessClubId);

            await _emailService.SendEmailAsync(
                $"{invitationRole} activation Link",
                recipientEmailAddress,
                recipientEmailAddress,
                $"Hello,<br />" +
                $"<br />" +
                $"you've been invited to the '{fitnessClub.FitnessClubName}' club for a {invitationRole} role. " +
                $"You can create an account using the following link: {invitationLink}<br />" +
                $"<br />" +
                $"With kindest regards,<br />" +
                $"MuscleUp Team");
        }
    }
}
