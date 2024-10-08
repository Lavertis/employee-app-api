﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    [MaxLength(64)] public required string FirstName { get; set; }
    [MaxLength(64)] public required string LastName { get; set; }
    public int Age { get; set; }

    public Sex? Sex { get; set; }
    public Guid SexId { get; set; }
}