using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Repo;
using Carnets.Repo.Repositories;
using Common.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ProgramHelper.AddBasicApiServices<Program>(builder.Services);

ProgramHelper.AddDbContext<CarnetsDbContext>(builder.Services, builder.Configuration);

builder.Services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
builder.Services.AddScoped<IPermissionRepository<AllowedEntriesPermission>, AllowedEntriesPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository<TimePermissionEntry>, TimePermissionEntryRepository>();

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