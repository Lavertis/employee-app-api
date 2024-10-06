namespace EmployeeApp.API.Dto.Employee.Requests;

public class BulkDeleteEmployeesRequest
{
    public required IList<Guid> EmployeeIds { get; set; }
}