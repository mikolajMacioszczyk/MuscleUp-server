using Common.Consts;
using Common.Helpers.Middleware;
using Common.Interfaces;
using Common.Models;
using Common.Resolvers;
using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Common.Extensions
{
    public static class ProgramExtensions
    {
        #region Configure services

        public static void AddBasicApiServices<TProgram>(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new IgnoreJsonPropertyAttributeContractResolver();
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    });
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            });

            services.AddScoped<HttpAuthContext>();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(TProgram));
            services.AddHttpClient();
        }

        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        public static void AddDbContext<TDbContext>(IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecret = configuration.GetValue<string>(AuthConsts.JwtSecretKey);
            if (string.IsNullOrWhiteSpace(jwtSecret))
            {
                throw new Exception("Jwt secret not configured!");
            }

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization();
            services.AddSingleton<IAuthorizationService, AuthService>();
        }

        #endregion


        #region Configure

        public static async Task MigrateDatabase<TDbContext>(this IServiceProvider services)
            where TDbContext : DbContext
        {
            int retryCount = 0;
            bool succeded = false;

            while (!succeded)
            {
                try
                {
                    using var scope = services.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
                    Console.WriteLine("Migrating database...");
                    await context.Database.MigrateAsync();
                    succeded = true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount == SeedConsts.RetryCount)
                    {
                        throw;
                    }

                    Console.WriteLine($"Failed to apply migration. Exception: {ex.Message}");
                    Console.WriteLine($"Attempty {retryCount}. Waiting {SeedConsts.WaitMiliseconds} ms");
                    Thread.Sleep(SeedConsts.WaitMiliseconds);
                }
            }
        }

        public static void UseExceptionMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionMiddleware();
            }
            else
            {
                app.UseExceptionMiddleware();
            }
        }

        public static void UseJwtAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<AuthTokenValidationMiddleware>();
        }

        #endregion
    }
}
