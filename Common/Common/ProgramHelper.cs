using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.API
{
    public static class ProgramHelper
    {
        public static void AddBasicApiServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static void AddDbContext<TDbContext>(IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static async Task MigrateDatabase<TDbContext>(IServiceProvider services)
            where TDbContext : DbContext
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            Console.WriteLine("Migrating database...");
            await context.Database.MigrateAsync();
        }
    }
}
