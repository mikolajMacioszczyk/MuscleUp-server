using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Domain.Services;
using Carnets.Domain.Services.Permission;
using Carnets.Repo;
using Carnets.Repo.Repositories;
using Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<CarnetsDbContext>(builder.Services, builder.Configuration);

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
builder.Services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
builder.Services.AddScoped<IPermissionService<ClassPermission>, ClassPermissionService>();
builder.Services.AddScoped<IPermissionRepository<PerkPermission>, PerkPermissionRepository>();
builder.Services.AddScoped<IPermissionService<PerkPermission>, PerkPermissionService>();
builder.Services.AddScoped<IAssignedPermissionRepository, AssignedPermissionRepository>();
builder.Services.AddScoped<IFitnessClubHttpService, FitnessClubHttpService>();
builder.Services.AddScoped<IGympassRepository, GympassRepository>();
builder.Services.AddScoped<IGympassService, GympassService>();
builder.Services.AddScoped<ISubscriptionService, MockSubscriptionService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IAssignedPermissionService, AssignedPermissionService>();
builder.Services.AddScoped<IGympassTypeService, GympassTypeService>();

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

                Console.WriteLine("Seeding default carnets data...");
                await CarnetsDbContextSeed.SeedDefaultCarnetsDataAsync(context);
                logger.LogInformation("Seed DONE");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}