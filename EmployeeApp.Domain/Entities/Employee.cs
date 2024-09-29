namespace EmployeeApp.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public Sex Sex { get; set; }
    public Guid SexId { get; set; }
}
