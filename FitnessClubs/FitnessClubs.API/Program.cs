using Common.Extensions;
using FitnessClubs.Application;
using FitnessClubs.Application.Interfaces;
using FitnessClubs.Repo;
using FitnessClubs.Repo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.AddApplicationServices();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<FitnessClubsDbContext>(builder.Services, builder.Configuration);

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// register app services
builder.Services.AddScoped<IFitnessClubRepository, FitnessClubRepository>();
builder.Services.AddScoped<IWorkerEmploymentRepository, WorkerEmploymentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

await app.Services.MigrateDatabase<FitnessClubsDbContext>();
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
                var context = services.GetRequiredService<FitnessClubsDbContext>();

            
                Console.WriteLine("Seeding default fitness club data...");
                await FitnessClubsDbContextSeed.SeedDefaultFitnessClubsDataAsync(context);
                logger.LogInformation("Seed DONE");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}