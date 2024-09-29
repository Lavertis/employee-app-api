using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Infrastructure.Database;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Sex> Sexes { get; set; }
}