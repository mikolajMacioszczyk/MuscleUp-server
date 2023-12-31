﻿using FitnessClubs.Application.Memberships.Handler;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace FitnessClubs.API.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var brokerSection = configuration.GetSection("Broker:Host");
            var consumptionExchangeSection = configuration.GetSection("Broker:ConsumptionExchange");
            var productionExchangeSection = configuration.GetSection("Broker:ProductionExchange");

            services.AddRabbitMqClient(brokerSection)
                .AddConsumptionExchange(Common.CommonConsts.MembershipExchangeName, consumptionExchangeSection)
                .AddAsyncMessageHandlerSingleton<NewMembershipHandler>(Common.CommonConsts.MembershipQueueName)
                .AddProductionExchange(Common.CommonConsts.TerminatedEmploymentExchangeName, productionExchangeSection);

            var isRabbitMqAvailable = false;
            var retryCount = 0;

            while (!isRabbitMqAvailable && retryCount++ < 10)
            {
                try
                {
                    var queue = services.BuildServiceProvider().GetRequiredService<IQueueService>();
                    queue.StartConsuming();
                    isRabbitMqAvailable = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
