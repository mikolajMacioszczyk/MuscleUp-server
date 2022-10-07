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
builder.Services.AddJwtAuthentication();

builder.Services.AddScoped<IGympassTypeRepository, GympassTypeRepository>();
builder.Services.AddScoped<IPermissionRepository<AllowedEntriesPermission>, AllowedEntriesPermissionRepository>();
builder.Services.AddScoped<IPermissionService<AllowedEntriesPermission>, AllowedEntriesPermissionService>();
builder.Services.AddScoped<IPermissionRepository<ClassPermission>, ClassPermissionRepository>();
builder.Services.AddScoped<IPermissionService<ClassPermission>, ClassPermissionService>();
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

app.UseHttpsRedirection();

app.UseJwtAuthentication();

app.MapControllers();

app.Run();