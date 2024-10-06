namespace EmployeeApp.API.Dto.Employee.Requests;

public class CreateEmployeeRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public Guid SexId { get; set; }
}