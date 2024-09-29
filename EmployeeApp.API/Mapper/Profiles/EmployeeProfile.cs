using EmployeeApp.API.Dto.Employee;
using EmployeeApp.API.Dto.Sex;

namespace EmployeeApp.API.Mapper.Profiles;

using AutoMapper;
using Domain.Entities;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDetailsResponse>()
            .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => new SexResponse { Id = src.Sex.Id, Name = src.Sex.Name }));
    }
}