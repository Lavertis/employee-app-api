namespace EmployeeApp.API.Dto.Employee;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Guid SexId { get; set; }
}