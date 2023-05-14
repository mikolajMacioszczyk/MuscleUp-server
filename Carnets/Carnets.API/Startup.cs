using Carnets.Application.Interfaces;
using Carnets.Application.Services;
using Carnets.Domain.Models;
using Carnets.Infrastructure.Services;
using Carnets.Repo.Repositories;
using Carnets.Repo;
using Common.Extensions;
using Common.Interfaces;
using Common.Services;
using Stripe;
using Carnets.Application;
using RabbitMQ.Client.Core.DependencyInjection;

namespace Carnets.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBasicApiServices<Program>();
            services.AddApplicationServices();
            services.ConfigureRouting();

            ProgramExtensions.AddDbContext<CarnetsDbContext>(services, Configuration);

            // Authentication
            services.AddJwtAuthentication(Configuration);

            StripeConfiguration.ApiKey = Configuration.GetValue<string>("StripeSecretKey");

            services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
            services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
            services.AddScoped<IPermissionRepository<PerkPermission>, PerkPermissionRepository>();
            services.AddScoped<IAssignedPermissionRepository, AssignedPermissionRepository>();
            services.AddScoped<IFitnessClubHttpService, FitnessClubHttpService>();
            services.AddScoped<IGympassRepository, GympassRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IAuthService, AuthHttpService>();
            services.AddScoped<IPaymentService, StripeService>();

            // RabbitMq
            services.AddRabbitMqClient(Configuration.GetSection("Broker:Host"))
                .AddProductionExchange(Common.CommonConsts.MembershipExchangeName, Configuration.GetSection("Broker:MembershipProductionExchange"))
                .AddProductionExchange(Common.CommonConsts.DeletedPermissionExchangeName, Configuration.GetSection("Broker:DeletedPermissionProductionExchange"));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionMiddleware();

            app.ApplicationServices.MigrateDatabase<CarnetsDbContext>().Wait();

            SeedDatabase(app.ApplicationServices, env).Wait();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJwtAuthentication();
             
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private async Task SeedDatabase(IServiceProvider serviceProvider, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger<Program>();

                    try
                    {
                        var context = services.GetRequiredService<CarnetsDbContext>();
                        var paymentService = services.GetRequiredService<IPaymentService>();

                        Console.WriteLine("Seeding default carnets data...");
                        await CarnetsDbContextSeed.SeedDefaultCarnetsDataAsync(context, paymentService);
                        logger.LogInformation("Seed DONE");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }
            }
        }
    }
}
