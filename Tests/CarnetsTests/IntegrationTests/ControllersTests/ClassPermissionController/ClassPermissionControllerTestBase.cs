using Carnets.API;
using Carnets.Application.Interfaces;
using CarnetsTests.IntegrationTests.ControllersTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CarnetsTests.IntegrationTests.ControllersTests.ClassPermissionController
{
    public class ClassPermissionControllerTestBase : ControllerTestBase
    {
        public ClassPermissionControllerTestBase(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        protected override void RegisterMockServices(IServiceCollection serviceCollection)
        {
            base.RegisterMockServices(serviceCollection);

            var fitnessClubService = serviceCollection.SingleOrDefault(db => db.ServiceType == typeof(IFitnessClubHttpService));
            if (fitnessClubService != null)
            {
                serviceCollection.Remove(fitnessClubService);
            }

            serviceCollection.AddScoped<IFitnessClubHttpService, MockFitnessClubService>();

            var queueService = serviceCollection.SingleOrDefault(db => db.ServiceType == typeof(IQueueService));
            if (queueService != null)
            {
                serviceCollection.Remove(queueService);
            }

            serviceCollection.AddScoped<IQueueService, MockQueueService>();
        }
    }
}
