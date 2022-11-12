using Common.Exceptions;
using Common.Interfaces;
using Common.Models;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Application.UserInvitations.Helpers;
using FitnessClubs.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitnessClubs.Application.UserInvitations.Commands
{
    public record ConsumeTrainerInvitationCommand(string InvitationId, string TrainerId) : IRequest<Result<TrainerEmployment>>
    { }

    public class ConsumeTrainerInvitationCommandHandler : IRequestHandler<ConsumeTrainerInvitationCommand, Result<TrainerEmployment>>
    {
        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IEmploymentRepository<TrainerEmployment> _trainerEmploymentRepository;
        private readonly ILogger<ConsumeTrainerInvitationCommandHandler> _logger;
        private readonly IAuthService _authService;

        public ConsumeTrainerInvitationCommandHandler(
            IUserInvitationRepository userInvitationRepository,
            IEmploymentRepository<TrainerEmployment> trainerEmploymentRepository,
            ILogger<ConsumeTrainerInvitationCommandHandler> logger,
            IAuthService authService)
        {
            _userInvitationRepository = userInvitationRepository;
            _logger = logger;
            _authService = authService;
            _trainerEmploymentRepository = trainerEmploymentRepository;
        }

        public async Task<Result<TrainerEmployment>> Handle(ConsumeTrainerInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitationResult = await ValidateTrainerInvitation(request.InvitationId, request.TrainerId);

            if (!invitationResult.IsSuccess)
            {
                return new Result<TrainerEmployment>(invitationResult.Errors);
            }
            var invitation = invitationResult.Value;

            var trainerEmployment = await InvitationHelper.CreateEmploymentFromInvitation(
                invitation, 
                request.TrainerId,
                new TrainerEmployment(),
                _trainerEmploymentRepository);

            var invitationUpdate = await InvitationHelper.SetInvitationUsed(invitation, _userInvitationRepository);

            try
            {
                await _trainerEmploymentRepository.SaveChangesAsync();
                await _userInvitationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error while saving changes in {nameof(ConsumeWorkerInvitationCommandHandler)}.\nError = {ex.Message}");
            }

            return new Result<TrainerEmployment>(trainerEmployment);
        }

        private async Task<Result<UserInvitation>> ValidateTrainerInvitation(string invitationId, string trainerId)
        {
            var invitation = await _userInvitationRepository.GetUserInvitationById(invitationId, true);

            if (invitation is null)
            {
                return new Result<UserInvitation>("Invalid invitation token");
            }

            if (invitation.ExpirationDateTime < DateTime.UtcNow)
            {
                return new Result<UserInvitation>("Invalid token expired");
            }

            if (invitation.IsUsed)
            {
                return new Result<UserInvitation>("Invalid token has been already used");
            }

            if (invitation.UserType != Common.Enums.RoleType.Trainer)
            {
                return new Result<UserInvitation>($"Cannot use invitation token for role = {invitation.UserType} to employ Trainer");
            }

            var trainersReult = (await _authService.GetAllTrainersWithIds(new string[] { trainerId }));
            if (!trainersReult.IsSuccess)
            {
                throw new BadRequestException(trainersReult.ErrorCombined);
            }
            var trainer = trainersReult.Value.FirstOrDefault();

            if (trainer is null)
            {
                return new Result<UserInvitation>($"Trainer with id = {trainer} does not exists");
            }

            if (trainer.Email != invitation.Email)
            {
                return new Result<UserInvitation>($"Trainer email address = {trainer.Email} does not match invitation email address");
            }

            var trainersFitnessClubs = await _trainerEmploymentRepository.GetAllFitnessClubsOfEmployee(trainerId, onlyActive: true, asTracking: false);
            if (trainersFitnessClubs.Any(t => t.FitnessClubId == invitation.FitnessClub.FitnessClubId))
            {
                return new Result<UserInvitation>($"Trainer with id = {trainerId} has active employment in FitnessClub with name: {invitation.FitnessClub.FitnessClubName}");
            }

            return new Result<UserInvitation>(invitation);
        }
    }
}
