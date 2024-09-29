using EmployeeApp.API.Mapper.Profiles;

namespace EmployeeApp.API.Mapper;

public static class AutoMapperModule
{
    public static void AddAutoMapperModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EmployeeProfile));
    }
}