using Carnets.Repo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CarnetsDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine($"ConnectionString: ${connectionString}");
    app.UseSwagger();
    app.UseSwaggerUI();
}

await MigrateDatabase(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task MigrateDatabase(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<CarnetsDbContext>();
    Console.WriteLine("Migrating database...");
    await context.Database.MigrateAsync();
}