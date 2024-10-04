namespace EmployeeApp.API.Dto.Employee;

public class CreateEmployeeRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public Guid SexId { get; set; }
}