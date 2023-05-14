using Carnets.API;
using Microsoft.AspNetCore;

namespace Carnets;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    private static IHost BuildWebHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>();
            })
            .Build();
    }
}
