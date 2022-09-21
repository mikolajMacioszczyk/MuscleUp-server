using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Repo;
using Auth.Repo.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Extensions;
using Auth.Domain.Managers;
using Auth.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using Auth.Domain.Dtos;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 5;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Authentication
builder.Services.AddJwtAuthentication();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Add services to the container.
builder.Services.AddBasicApiServices<Program>();
builder.Services.ConfigureRouting();

// Application services
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<IAccountHttpManager, AccountHttpManager>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISpecificUserRepository<Member, RegisterMemberDto>, MemberRepository>();
builder.Services.AddScoped<ISpecificUserRepository<Worker, RegisterWorkerDto>, WorkerRepository>();
builder.Services.AddScoped<ISpecificUserRepository<Trainer, RegisterTrainerDto>, TrainerRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<IAuthTokenManager, AuthTokenManager>();
builder.Services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

await app.Services.MigrateDatabase<AuthDbContext>();
await SeedDatabase(app.Services);

app.UseHttpsRedirection();

app.UseJwtAuthentication();

app.MapControllers();

app.Run();

static async Task SeedDatabase(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();

        try
        {
            var context = services.GetRequiredService<AuthDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            logger.LogInformation("Seeding roles..");
            await AuthDbContextSeed.SeedRolesAsync(roleManager);
            logger.LogInformation("Seeding admin...");
            await AuthDbContextSeed.SeedAdminAsync(userManager, context);
            logger.LogInformation("Seed DONE");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
