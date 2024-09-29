namespace EmployeeApp.API.Dto.Employee;

public class UpdateEmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? Age { get; set; }
    public Guid? SexId { get; set; }
}