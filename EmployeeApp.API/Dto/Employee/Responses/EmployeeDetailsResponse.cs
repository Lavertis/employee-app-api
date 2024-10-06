using EmployeeApp.API.Dto.Sex;

namespace EmployeeApp.API.Dto.Employee.Responses;

public class EmployeeDetailsResponse
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public required SexResponse Sex { get; set; }
}