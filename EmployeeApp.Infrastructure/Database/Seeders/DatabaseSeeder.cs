using EmployeeApp.Domain.Entities;

namespace EmployeeApp.Infrastructure.Database.Seeders
{
    public class DatabaseSeeder
    {
        private readonly EmployeeDbContext _context;

        public DatabaseSeeder(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task SeedEmployeeDb()
        {
            RemoveData();
            await SeedData();
        }

        private void RemoveData()
        {
            _context.RemoveRange(_context.Employees);
            _context.RemoveRange(_context.Sexes);
            _context.SaveChanges();
        }

        private async Task SeedData()
        {
            await SeedSexes();
            await SeedEmployees();
        }

        private async Task SeedEmployees()
        {
            var sexes = _context.Sexes.ToList();
            var employees = new List<Employee>
            {
                new() { FirstName = "John", LastName = "Doe", Age = 23, Sex = sexes[0] },
                new() { FirstName = "Jane", LastName = "Doe", Age = 28, Sex = sexes[1] },
                new() { FirstName = "Alex", LastName = "Smith", Age = 35, Sex = sexes[2] },
            };
            await _context.AddRangeAsync(employees);
            await _context.SaveChangesAsync();
        }

        private async Task SeedSexes()
        {
            var sexList = new List<Sex>
            {
                new() { Name = "MALE" },
                new() { Name = "FEMALE" },
                new() { Name = "OTHER" }
            };

            await _context.AddRangeAsync(sexList);
            await _context.SaveChangesAsync();
        }
    }
}
