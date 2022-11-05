using Carnets.Application.FitnessClubs.Queries;
using Carnets.Application.Interfaces;
using Carnets.Domain.Models;
using Common.Enums;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Carnets.Application.Subscriptions.Commands
{
    public record CancellAllGympassSubscriptionsCommand(string GympassId) : IRequest<Result<IEnumerable<Subscription>>>
    { }

    public class CancellAllGympassSubscriptionsCommandHandler : IRequestHandler<CancellAllGympassSubscriptionsCommand, Result<IEnumerable<Subscription>>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IGympassRepository _gympassRepository;
        private readonly ILogger<CancellSubscriptionCommandHandler> _logger;
        private readonly ISender _mediator;
        private readonly HttpAuthContext _httpAuthContext;

        public CancellAllGympassSubscriptionsCommandHandler(
            ISubscriptionRepository subscriptionRepository,
            ILogger<CancellSubscriptionCommandHandler> logger,
            IGympassRepository gympassRepository,
            HttpAuthContext httpAuthContext,
            ISender mediator)
        {
            _subscriptionRepository = subscriptionRepository;
            _gympassRepository = gympassRepository;
            _httpAuthContext = httpAuthContext;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Result<IEnumerable<Subscription>>> Handle(CancellAllGympassSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateManagementPermissions(request.GympassId);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var activeSubscriptions = await GetActiveGympassSubscriptions(request.GympassId);

            var cancelledSubscriptions = await CancellSelectedSubscriptions(activeSubscriptions);

            return new Result<IEnumerable<Subscription>>(cancelledSubscriptions);
        }

        private async Task<Result<IEnumerable<Subscription>>> ValidateManagementPermissions(string gympassId)
        {
            var gympass = await _gympassRepository.GetById(gympassId, false);

            if (gympass is null ||
                // member attempts to cancel other's subscriptions
                _httpAuthContext.UserRole == RoleType.Member && gympass.UserId != _httpAuthContext.UserId)
            {
                return new Result<IEnumerable<Subscription>>(Common.CommonConsts.NOT_FOUND);
            }
            else if (_httpAuthContext.UserRole == RoleType.Worker || _httpAuthContext.UserRole == RoleType.Owner)
            {
                var workerFitnessClub = await _mediator.Send(
                    new EnsureWorkerCanManageFitnessClubQuery() { WorkerId = _httpAuthContext.UserId });
                // worker attempts to cancel subscriptions he has no permission to manage
                if (workerFitnessClub.FitnessClubId != gympass.GympassType.FitnessClubId)
                {
                    return new Result<IEnumerable<Subscription>>(Common.CommonConsts.NOT_FOUND);
                }
            }

            return new Result<IEnumerable<Subscription>>(null as IEnumerable<Subscription>);
        }

        private async Task<IEnumerable<Subscription>> CancellSelectedSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            var cancelledSubscriptions = new List<Subscription>();

            foreach (var subscription in subscriptions)
            {
                var cancelResult = await _mediator.Send(new CancellSubscriptionCommand(subscription.SubscriptionId));

                if (cancelResult.IsSuccess)
                {
                    cancelledSubscriptions.Add(cancelResult.Value);
                }
                else
                {
                    _logger.LogError($"Error while cancelling subscription with id = {subscription.SubscriptionId}");
                }
            }

            return cancelledSubscriptions;
        }

        private async Task<IEnumerable<Subscription>> GetActiveGympassSubscriptions(string gympassId)
        {
            var allSubscriptions = await _subscriptionRepository.GetAllGympassSubscriptions(new[] { gympassId }, true);

            return allSubscriptions.Where(s => s.IsActive);
        }
    }
}
