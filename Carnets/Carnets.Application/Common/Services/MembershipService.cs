using Carnets.Application.Interfaces;
using Common.Models.Dtos;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace Carnets.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IQueueService _queueService;

        public MembershipService(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public async Task CreateMembership(CreateMembershipDto membershipDto)
        {
            await _queueService.SendAsync(
                @object: membershipDto,
                exchangeName: Common.CommonConsts.ExchangeName,
                routingKey: Common.CommonConsts.MembershipQueueName
                );
        }
    }
}
