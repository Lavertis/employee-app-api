using System.Reflection;
using FluentValidation;

namespace EmployeeApp.API.Validators;

public static class FluentValidationModule
{
    public static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}