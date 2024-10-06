namespace EmployeeApp.API.Dto.Employee.Responses;

public class EmployeeListItemResponse
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int Age { get; set; }
    public required string Sex { get; set; }
}