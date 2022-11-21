using Carnets.Application;
using Carnets.Application.Interfaces;
using Carnets.Application.Services;
using Carnets.Domain.Models;
using Carnets.Infrastructure.Services;
using Carnets.Repo;
using Carnets.Repo.Repositories;
using Common.Extensions;
using Common.Interfaces;
using Common.Services;
using RabbitMQ.Client.Core.DependencyInjection;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.AddApplicationServices();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<CarnetsDbContext>(builder.Services, builder.Configuration);

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSecretKey");

builder.Services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
builder.Services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository<PerkPermission>, PerkPermissionRepository>();
builder.Services.AddScoped<IAssignedPermissionRepository, AssignedPermissionRepository>();
builder.Services.AddScoped<IFitnessClubHttpService, FitnessClubHttpService>();
builder.Services.AddScoped<IGympassRepository, GympassRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();
builder.Services.AddScoped<IAuthService, AuthHttpService>();
builder.Services.AddScoped<IPaymentService, StripeService>();

// RabbitMq
builder.Services.AddRabbitMqClient(builder.Configuration.GetSection("Broker:Host"))
    .AddProductionExchange(Common.CommonConsts.MembershipExchangeName, builder.Configuration.GetSection("Broker:MembershipProductionExchange"))
    .AddProductionExchange(Common.CommonConsts.DeletedPermissionExchangeName, builder.Configuration.GetSection("Broker:DeletedPermissionProductionExchange"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

await app.Services.MigrateDatabase<CarnetsDbContext>();
await SeedDatabase(app.Services, app.Environment);

app.UseHttpsRedirection();

app.UseJwtAuthentication();

app.MapControllers();

app.Run();

static async Task SeedDatabase(IServiceProvider serviceProvider, IWebHostEnvironment environment)
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