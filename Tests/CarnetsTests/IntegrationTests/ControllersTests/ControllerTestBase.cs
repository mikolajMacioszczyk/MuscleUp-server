using Carnets.API;
using Carnets.Application.Interfaces;
using Carnets.Repo;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xunit;

namespace CarnetsTests.IntegrationTests.ControllersTests
{
    public abstract class ControllerTestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly CustomWebApplicationFactory<Startup> _factory;
        protected readonly HttpClient _client;

        protected const string AdminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdXBlcmFkbWluIiwianRpIjoiMmZkNDI0NzYtZjJjNi00NDg5LTgxZTctMzhhMjZiNTExOWE0Iiwicm9sZSI6IkFkbWluaXN0cmF0b3IiLCJleHAiOjE3MTU2MDQyNjYsInVzZXJJZCI6IjU2YzMzODllLThmZWEtNDUzZS1hY2MyLWU1M2EyZTI3NjhiNyIsImZpcnN0TmFtZSI6IkpvaG4iLCJsYXN0TmFtZSI6IlJhbWJvIiwidG9rZW5UeXBlIjoiQWNjZXNzVG9rZW4ifQ._NMhvcl4aFrNropeY7JekvPXxfTStJyVL4mW7qEus-fel9-xC3yfO6S9lIaIqnYG7gKT-Ie86EvfOWle6KyvUA";
        protected const string WorkerToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ3b3JrZXIiLCJqdGkiOiJhZjFjZmEwNi05ZmQ3LTQ0OGItODg1Ny05ZmQyMTE1OGNjMWIiLCJyb2xlIjoiV29ya2VyIiwiZXhwIjoxNzE1NjA0MjgwLCJ1c2VySWQiOiI3NDdmNmM1MC1hYWJjLTRiZTItYWE0My0xMGNmNjBhYmE0NmIiLCJmaXJzdE5hbWUiOiJDaGFybGVzIiwibGFzdE5hbWUiOiJMZWNsZXJjIiwidG9rZW5UeXBlIjoiQWNjZXNzVG9rZW4ifQ.Jxd1xufd-Vn7Didclxhykp068JKcJOLQzQ1FXrfGiIAqCv0K4o8KOiiDuxyj18I38w8az9owysZ7DiAhWTGvtQ";
        protected const string OwnerToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJvd25lciIsImp0aSI6Ijc0MTQ3Nzg2LTg1MzgtNDU4ZS04MWEyLTdmYjVhNWZhMjI2NSIsInJvbGUiOiJPd25lciIsImV4cCI6MTcxNTYwNDI0NCwidXNlcklkIjoiNjQ0YzZkMGEtYjY2Yi00MzMyLWE5NTQtYzljY2M3ZWM5NWY2IiwiZmlyc3ROYW1lIjoiU2VyZ2lvIiwibGFzdE5hbWUiOiJQZXJleiIsInRva2VuVHlwZSI6IkFjY2Vzc1Rva2VuIn0.PCjdXN1ct--sGD2hrYVxRLVjkcow52b7T9Xbda9TfgOUAqpHOahWRM1-ftic244xX5pEo5AEwWHqA0Vs0r8X7g";
        protected const string TrainerToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0cmFpbmVyIiwianRpIjoiZTA2NzEyN2ItY2E5Yy00YzA1LWI3ODMtODAzNTdhMTEzMWYyIiwicm9sZSI6IlRyYWluZXIiLCJleHAiOjE3MTU2MDQyOTMsInVzZXJJZCI6ImM3YzI1MDI0LTE5OTctNGM5Yi05ZTM3LWZlMTUwZDBmYWFjMSIsImZpcnN0TmFtZSI6Ik1heCIsImxhc3ROYW1lIjoiVmVyc3RhcHBlbiIsInRva2VuVHlwZSI6IkFjY2Vzc1Rva2VuIn0.zgguPOkFWKToKXF6-sXHkrMkKYgUfoqrrbdDpdx3J_yKdCoeCxXOYxMEGtz6tYOnleiD4B3ISCTy58sJ84S0uw";
        protected const string MemberToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtZW1iZXIiLCJqdGkiOiIwNTAzMjUzNy1mOTE2LTQwYWQtYmQ2Mi1hMWM4Mjk0OTRkMGYiLCJyb2xlIjoiTWVtYmVyIiwiZXhwIjoxNzE1NjA0MzY5LCJ1c2VySWQiOiJmYWYzMTRiYy0xMTc1LTQ2YjAtYTU3Yy1kNDU2ZjA5Y2ExMzkiLCJmaXJzdE5hbWUiOiJMZXdpcyIsImxhc3ROYW1lIjoiSGFtaWx0b24iLCJ0b2tlblR5cGUiOiJBY2Nlc3NUb2tlbiJ9.4zn1Q5s9PKkby76iGCqqWzFGoGLkF2W_sPCx2f8xSUH2nmRUNTQ9EY4WZID14JhyzUq8LVndLWAGDwlvi1mflQ";

        public ControllerTestBase(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _factory.DbName = $"IntegrationTests_{Guid.NewGuid()}";
            _factory.CustomServicesConfiguration = RegisterMockServices;

            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContextScoped = scopedServices.GetRequiredService<CarnetsDbContext>();
                SeedDatabase(dbContextScoped).Wait();

                dbContextScoped.Database.EnsureCreated();
                dbContextScoped.SaveChanges();
            }

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        protected virtual async Task SeedDatabase(CarnetsDbContext context)
        {
            var paymentServiceMock = new Mock<IPaymentService>();

            Console.WriteLine("Seeding default carnets data...");
            await CarnetsDbContextSeed.SeedDefaultCarnetsDataAsync(context, paymentServiceMock.Object);
            Console.WriteLine("Seeding done");
        }

        protected virtual void RegisterMockServices(IServiceCollection serviceCollection)
        {
        }

        protected async Task<T> DeserializeMessageContent<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected void AddBearerToken(string token)
        {
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {token}");
        }
    }
}
