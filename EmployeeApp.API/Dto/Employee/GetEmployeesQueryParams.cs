using EmployeeApp.API.Dto.Query;

namespace EmployeeApp.API.Dto.Employee;

public class GetEmployeesQueryParams : PaginationQuery
{
    public string? FullName { get; set; }
}