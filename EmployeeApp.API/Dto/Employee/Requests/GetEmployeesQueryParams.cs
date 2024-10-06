using EmployeeApp.API.Dto.Query;

namespace EmployeeApp.API.Dto.Employee.Requests;

public class GetEmployeesQueryParams : PaginationQuery
{
    public string? FullName { get; set; }
}