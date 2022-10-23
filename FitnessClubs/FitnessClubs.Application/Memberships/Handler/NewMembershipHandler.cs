using Common.Models.Dtos;
using FitnessClubs.Application.Memberships.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;

namespace FitnessClubs.Application.Memberships.Handler
{
    public class NewMembershipHandler : IAsyncMessageHandler
    {
        private readonly ILogger<NewMembershipHandler> _logger;
        private readonly ISender _mediator;

        public NewMembershipHandler(ILogger<NewMembershipHandler> logger, ISender mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        {
            CreateMembershipDto payload = null;

            try
            {
                payload = JsonConvert.DeserializeObject<CreateMembershipDto>(eventArgs.GetMessage());
                _logger.LogInformation($"Handler {nameof(NewMembershipHandler)} - susccessfouly deserialized message");
            }
            catch (Exception)
            {
                _logger.LogError($"Handler {nameof(NewMembershipHandler)}: cannot deserialize message {eventArgs.GetMessage()}");
                return;
            }

            var membershipResult = await _mediator.Send(new CreateOrGetMembershipCommand()
            {
                CreateMembershipDto = payload,
                UserConfirmed = true
            });

            if (membershipResult.IsSuccess)
            {
                _logger.LogInformation($"Handler {nameof(NewMembershipHandler)} - membership successfouly established");
            }
            else
            {
                _logger.LogWarning($"Handler {nameof(NewMembershipHandler)} - membership creation failed. Reason: {membershipResult.ErrorCombined}");
            }
        }
    }
}
