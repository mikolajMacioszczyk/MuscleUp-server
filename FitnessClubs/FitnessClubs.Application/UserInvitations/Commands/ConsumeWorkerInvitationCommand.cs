using Common.BaseClasses;
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
    public record ConsumeWorkerInvitationCommand(string InvitationId, string WorkerId) : IRequest<Result<WorkerEmployment>>
    { }

    public class ConsumeWorkerInvitationCommandHandler : ConcurrentTokenHandlerBase,
        IRequestHandler<ConsumeWorkerInvitationCommand, Result<WorkerEmployment>>
    {

        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IEmploymentRepository<WorkerEmployment> _workerEmploymentRepository;
        private readonly ILogger<ConsumeWorkerInvitationCommandHandler> _logger;
        private readonly IAuthService _authService;

        public ConsumeWorkerInvitationCommandHandler(
            IUserInvitationRepository userInvitationRepository,
            IEmploymentRepository<WorkerEmployment> workerEmploymentRepository,
            ILogger<ConsumeWorkerInvitationCommandHandler> logger,
            IAuthService authService)
        {
            _userInvitationRepository = userInvitationRepository;
            _workerEmploymentRepository = workerEmploymentRepository;
            _logger = logger;
            _authService = authService;
        }

        public async Task<Result<WorkerEmployment>> Handle(ConsumeWorkerInvitationCommand request, CancellationToken cancellationToken)
        {
            while (!LockToken(request.InvitationId))
            {
                Thread.Sleep(WaitMiliseconds);
            }

            try
            {
                var result = await HandleWithoutConcurrency(request, cancellationToken);
                ReleaseToken(request.InvitationId);
                return result;
            }
            catch (Exception)
            {
                ReleaseToken(request.InvitationId);
                throw;
            }
        }

        private async Task<Result<WorkerEmployment>> HandleWithoutConcurrency(ConsumeWorkerInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitationResult = await ValidateWorkerInvitation(request.InvitationId, request.WorkerId);

            if (!invitationResult.IsSuccess)
            {
                return new Result<WorkerEmployment>(invitationResult.Errors);
            }
            var invitation = invitationResult.Value;

            var workerEmployment = await InvitationHelper.CreateEmploymentFromInvitation(
                invitation, 
                request.WorkerId,
                new WorkerEmployment(),
                _workerEmploymentRepository);

            var invitationUpdate = await InvitationHelper.SetInvitationUsed(invitation, _userInvitationRepository);

            try
            {
                await _workerEmploymentRepository.SaveChangesAsync();
                await _userInvitationRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error while saving changes in {nameof(ConsumeWorkerInvitationCommandHandler)}.\nError = {ex.Message}");
            }

            return new Result<WorkerEmployment>(workerEmployment);
        }

        private async Task<Result<UserInvitation>> ValidateWorkerInvitation(string invitationId, string workerId)
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

            if (invitation.UserType != Common.Enums.RoleType.Worker)
            {
                return new Result<UserInvitation>($"Cannot use invitation token for role = {invitation.UserType} to employ Worker");
            }

            var workersResult = (await _authService.GetAllWorkersWithIds(new string[] { workerId }));
            if (!workersResult.IsSuccess)
            {
                throw new InvalidInputException(workersResult.ErrorCombined);
            }
            var worker = workersResult.Value.FirstOrDefault();

            if (worker is null)
            {
                return new Result<UserInvitation>($"Worker with id = {workerId} does not exists");
            }

            if (worker.Email != invitation.Email)
            {
                return new Result<UserInvitation>($"Worker email address = {worker.Email} does not match invitation email address");
            }

            var workersFitnessClubs = await _workerEmploymentRepository.GetAllFitnessClubsOfEmployee(workerId, onlyActive: true, asTracking: false);
            if (workersFitnessClubs.Any())
            {
                return new Result<UserInvitation>($"Worker with id = {workerId} has active employment");
            }

            return new Result<UserInvitation>(invitation);
        }
    }
}
