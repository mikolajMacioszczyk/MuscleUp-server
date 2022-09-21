using Common.Extensions;
using FitnessClubs.Domain.Interfaces;
using FitnessClubs.Repo;
using FitnessClubs.Repo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<FitnessClubsDbContext>(builder.Services, builder.Configuration);

// Authentication
builder.Services.AddJwtAuthentication();

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

app.UseHttpsRedirection();

app.UseJwtAuthentication();

app.MapControllers();

app.Run();