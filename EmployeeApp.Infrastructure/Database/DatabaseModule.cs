using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeApp.Infrastructure.Database;

public static class DatabaseModule
{
    public static void AddDatabaseModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(
            GetConnectionString(configuration),
            x => x.MigrationsAssembly("EmployeeApp.Infrastructure")
        ));
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EmployeeDB");
        if (connectionString == null)
            throw new Exception("Cannot get EmployeeDB connection string");
        return connectionString;
    }
}