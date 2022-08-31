using Common.API;
using FitnessClubs.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ProgramHelper.AddBasicApiServices<Program>(builder.Services);

ProgramHelper.AddDbContext<FitnessClubsDbContext>(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await ProgramHelper.MigrateDatabase<FitnessClubsDbContext>(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();