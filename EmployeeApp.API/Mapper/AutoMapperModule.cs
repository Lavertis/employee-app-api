using AutoMapper;
using EmployeeApp.API.Dto.Employee;
using EmployeeApp.API.Dto.Sex;
using EmployeeApp.Domain.Entities;

namespace EmployeeApp.API.Mapper;

public static class AutoMapperModule
{
    public static void AddAutoMapperModule(this IServiceCollection services)
    {
        var mapper = CreateAutoMapper();
        services.AddSingleton(mapper);
    }
    
    public static IMapper CreateAutoMapper()
    {
        var mapperConfiguration = new MapperConfiguration(options =>
        {
            options.CreateMap<Sex, SexResponse>();
            options.CreateMap<Employee, EmployeeDetailsResponse>();
            options.CreateMap<Employee, EmployeeListItemResponse>();
        });

        var mapper = mapperConfiguration.CreateMapper();
        return mapper;
    }
}