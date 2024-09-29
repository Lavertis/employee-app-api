using EmployeeApp.API.Dto.Sex;

namespace EmployeeApp.API.Dto.Employee;

public class EmployeeDetailsResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public SexResponse Sex { get; set; }
}