using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeApp.Infrastructure.Database.Seeders;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<EmployeeDbContext>();
        var seeder = new DatabaseSeeder(context);
        await seeder.SeedEmployeeDb();
    }
}