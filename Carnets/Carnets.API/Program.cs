using Carnets.Domain.Interfaces;
using Carnets.Domain.Models;
using Carnets.Repo;
using Carnets.Repo.Repositories;
using Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBasicApiServices<Program>();
builder.Services.ConfigureRouting();

ProgramExtensions.AddDbContext<CarnetsDbContext>(builder.Services, builder.Configuration);

builder.Services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
builder.Services.AddScoped<IPermissionRepository<AllowedEntriesPermission>, AllowedEntriesPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository<TimePermissionEntry>, TimePermissionEntryRepository>();
builder.Services.AddScoped<IAssignedPermissionRepository, AssignedPermissionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

await app.Services.MigrateDatabase<CarnetsDbContext>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();