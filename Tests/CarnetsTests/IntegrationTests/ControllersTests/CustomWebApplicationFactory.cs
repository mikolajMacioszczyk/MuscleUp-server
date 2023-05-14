using Carnets.Repo;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

namespace CarnetsTests.IntegrationTests.ControllersTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public string DbName { get; set; }

    public Action<IServiceCollection> CustomServicesConfiguration { get; set; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(serviceCollection =>
        {
            var dbContextOptions = serviceCollection.SingleOrDefault(db => db.ServiceType == typeof(DbContextOptions<CarnetsDbContext>));
            if (dbContextOptions != null)
            {
                serviceCollection.Remove(dbContextOptions);
            }
            
            var dbContext = serviceCollection.SingleOrDefault(db => db.ServiceType == typeof(DbConnection));
            if (dbContext != null)
            {
                serviceCollection.Remove(dbContext);
            }

            serviceCollection.AddDbContext<CarnetsDbContext>((container, options) =>
            {
                options.UseInMemoryDatabase(databaseName: DbName);
            });

            CustomServicesConfiguration?.Invoke(serviceCollection);
        });

        builder.Configure(app =>
        {
            app.UseExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJwtAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        });

        builder.UseEnvironment("Development");
    }
}