using Common.Extensions;
using FitnessClubs.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<FitnessClubsDbContext>(builder.Services, builder.Configuration);

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

app.UseAuthorization();

app.MapControllers();

app.Run();