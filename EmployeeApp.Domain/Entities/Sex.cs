using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Domain.Entities;

public class Sex
{
    public Guid Id { get; set; }

    [MaxLength(32)]
    public string Name { get; set; }
}