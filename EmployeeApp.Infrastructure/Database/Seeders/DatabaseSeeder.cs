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
            var firstNames = new List<string> { "John", "Jane", "Alex", "Chris", "Pat", "Taylor", "Jordan", "Morgan", "Casey", "Riley" };
            var lastNames = new List<string> { "Doe", "Smith", "Johnson", "Brown", "Williams", "Jones", "Garcia", "Miller", "Davis", "Rodriguez" };
            var random = new Random();
            var employees = new List<Employee>();

            for (var i = 0; i < 100; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Count)];
                var lastName = lastNames[random.Next(lastNames.Count)];
                var age = random.Next(20, 60);
                var sex = sexes[random.Next(sexes.Count)];

                employees.Add(new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Sex = sex
                });
            }

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
