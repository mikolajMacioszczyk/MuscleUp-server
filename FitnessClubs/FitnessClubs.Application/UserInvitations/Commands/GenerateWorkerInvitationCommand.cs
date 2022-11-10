using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FitnessClubs.Application.UserInvitations.Commands
{
    public record GenerateWorkerInvitationCommand(string FitnessClubId, string Email, string BaseInvitationLink)
        : IRequest<Result<UserInvitation>>
    { }

    public class GenerateWorkerInvitationCommandHandler : IRequestHandler<GenerateWorkerInvitationCommand, Result<UserInvitation>>
    {
        private const string InvitationTokenParamName = "token";
        private const string FitnessClubParamName = "fitness-club"; 
        
        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IFitnessClubRepository _fitnessClubRepository;
        private readonly IEmailService _emailService;
        private readonly int _invitationValidityInDays;

        public GenerateWorkerInvitationCommandHandler(
            IUserInvitationRepository userInvitationRepository,
            IFitnessClubRepository fitnessClubRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userInvitationRepository = userInvitationRepository;
            _fitnessClubRepository = fitnessClubRepository;

            _invitationValidityInDays = configuration.GetValue<int>("InvitationValidityInDays");
            _emailService = emailService;
        }

        public async Task<Result<UserInvitation>> Handle(GenerateWorkerInvitationCommand request, CancellationToken cancellationToken)
        {
            var fitnessClub = await _fitnessClubRepository.GetById(request.FitnessClubId, true);

            if (fitnessClub is null)
            {
                return new Result<UserInvitation>($"Fitness club with id = {request.FitnessClubId} does not exists");
            }

            // TODO: Validate if user with provided email does not exists. Will be implemented later

            var invitation = new UserInvitation()
            {
                Email = request.Email,
                FitnessClub = fitnessClub,
                IsUsed = false,
                UserType = Common.Enums.RoleType.Worker,
                ExpirationDateTime = DateTime.UtcNow.AddDays(_invitationValidityInDays),
            };

            var created = await _userInvitationRepository.CreateUserInvitation(invitation);

            await _userInvitationRepository.SaveChangesAsync();

            await SendInvitationEmail(request.Email, created.InvitationId, fitnessClub, request.BaseInvitationLink);

            return new Result<UserInvitation>(created);
        }

        private async Task SendInvitationEmail(
            string recipientEmailAddress, 
            string invitationId,
            FitnessClub fitnessClub,
            string baseInvitationLink)
        {
            var invitationLink = baseInvitationLink
                .AppendQueryParamToUri(InvitationTokenParamName, invitationId)
                .AppendQueryParamToUri(FitnessClubParamName, fitnessClub.FitnessClubId);

            await _emailService.SendEmailAsync(
                "Link aktywacyjny",
                recipientEmailAddress,
                "Zespół MuscleUp",
                $"Dzień dobry,<br />" +
                $"<br />" +
                $"Zaproszenie do stworzenia konta pracownika w klubie \"{fitnessClub.FitnessClubName}\".<br />" +
                $"Twój link: {invitationLink}<br />" +
                $"<br />" +
                $"Pozdrawiamy,<br />" +
                $"Zespół MuscleUp");
        }
    }
}
