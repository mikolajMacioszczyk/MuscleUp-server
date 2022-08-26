using Carnets.Repo;
using Common.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ProgramHelper.AddBasicApiServices(builder.Services);

ProgramHelper.AddDbContext<CarnetsDbContext>(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await ProgramHelper.MigrateDatabase<CarnetsDbContext>(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();