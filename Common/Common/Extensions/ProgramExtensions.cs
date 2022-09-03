using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common.Extensions
{
    public static class ProgramExtensions
    {
        public static void AddBasicApiServices<TProgram>(this IServiceCollection services)
        {
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(TProgram));
        }

        public static void AddDbContext<TDbContext>(IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static async Task MigrateDatabase<TDbContext>(this IServiceProvider services)
            where TDbContext : DbContext
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            Console.WriteLine("Migrating database...");
            await context.Database.MigrateAsync();
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
    }
}
