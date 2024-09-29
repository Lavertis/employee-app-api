namespace EmployeeApp.API.Config;

public static class CorsModule
{
    public static IApplicationBuilder UseCorsModule(this IApplicationBuilder app)
    {
        return app.UseCors(options => options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
        );
    }
}