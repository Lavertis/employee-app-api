namespace EmployeeApp.API.Dto.Employee;

public class EmployeeListItemResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
}