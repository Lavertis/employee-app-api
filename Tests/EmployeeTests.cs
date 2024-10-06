using EmployeeApp.API.CQRS.Commands.Employees;
using EmployeeApp.API.CQRS.Queries.Employees;
using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.API.Validators;
using EmployeeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class EmployeeTests : BaseTest
{
    [Fact]
    public async Task GetEmployeeQueryHandler_ReturnsEmployeeById_ForExistingEmployee()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        const string firstName = "John";
        const string lastName = "Doe";
        const int age = 30;
        var sexId = Guid.NewGuid();
        Context.Sexes.Add(
            new Sex { Id = sexId, Name = "Male" }
        );
        Context.Employees.Add(
            new Employee { Id = employeeId, FirstName = firstName, LastName = lastName, Age = age, SexId = sexId }
        );
        await Context.SaveChangesAsync();

        var handler = new GetEmployeeQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(new GetEmployeeQuery(employeeId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsError);
        Assert.Equal(employeeId, result.Value?.Id);
        Assert.Equal(firstName, result.Value?.FirstName);
        Assert.Equal(lastName, result.Value?.LastName);
        Assert.Equal(age, result.Value?.Age);
        Assert.Equal(sexId, result.Value?.Sex.Id);
    }

    [Fact]
    public async Task GetEmployeeQueryHandler_ReturnsNotFound_ForNonExistingEmployee()
    {
        // Arrange
        var handler = new GetEmployeeQueryHandler(Context, Mapper);

        // Act
        var result = await handler.Handle(new GetEmployeeQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsError);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task CreateEmployeeCommandHandler_CreatesNewEmployee_ForValidRequest()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const int age = 30;
        var sexId = Guid.NewGuid();
        Context.Sexes.Add(
            new Sex { Id = sexId, Name = "Male" }
        );
        await Context.SaveChangesAsync();
        var createEmployeeRequest = new CreateEmployeeRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Age = age,
            SexId = sexId
        };
        var handler = new CreateEmployeeCommandHandler(Context, new CreateEmployeeRequestValidator(Context));

        // Act
        var result = await handler.Handle(new CreateEmployeeCommand(createEmployeeRequest), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsError);
        Assert.False(result.HasValidationErrors);
        var employee = await Context.Employees
            .FirstOrDefaultAsync(e => e.Id == result.Value!.Id);
        Assert.NotNull(employee);
        Assert.Equal(firstName, employee.FirstName);
        Assert.Equal(lastName, employee.LastName);
        Assert.Equal(age, employee.Age);
        Assert.Equal(sexId, employee.SexId);
    }

    [Fact]
    public async Task CreateEmployeeCommandHandler_ReturnsBadRequest_ForInvalidRequest()
    {
        // Arrange
        var createEmployeeRequest = new CreateEmployeeRequest { FirstName = "John", LastName = "Doe" };
        var handler = new CreateEmployeeCommandHandler(Context, new CreateEmployeeRequestValidator(Context));

        // Act
        var result = await handler.Handle(new CreateEmployeeCommand(createEmployeeRequest), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.HasValidationErrors);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeCommandHandler_UpdatesEmployee_ForValidRequest()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        const string firstName = "John";
        const string lastName = "Doe";
        const int age = 30;
        var sexId = Guid.NewGuid();
        Context.Sexes.Add(
            new Sex { Id = sexId, Name = "Male" }
        );
        Context.Employees.Add(
            new Employee { Id = employeeId, FirstName = firstName, LastName = lastName, Age = age, SexId = sexId }
        );
        await Context.SaveChangesAsync();
        var updateEmployeeRequest = new UpdateEmployeeRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Age = 35,
            SexId = sexId
        };
        var handler = new UpdateEmployeeCommandHandler(Context, new UpdateEmployeeRequestValidator(Context));

        // Act
        var result = await handler.Handle(new UpdateEmployeeCommand(employeeId, updateEmployeeRequest),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsError);
        Assert.False(result.HasValidationErrors);
        var employee = await Context.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        Assert.NotNull(employee);
        Assert.Equal("Jane", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(35, employee.Age);
        Assert.Equal(sexId, employee.SexId);
    }

    [Fact]
    public async Task DeleteEmployeeCommandHandler_DeletesEmployee_ForExistingEmployeeWithoutReferences()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        const string firstName = "John";
        const string lastName = "Doe";
        const int age = 30;
        var sexId = Guid.NewGuid();
        Context.Sexes.Add(
            new Sex { Id = sexId, Name = "Male" }
        );
        Context.Employees.Add(
            new Employee { Id = employeeId, FirstName = firstName, LastName = lastName, Age = age, SexId = sexId }
        );
        await Context.SaveChangesAsync();
        var handler = new DeleteEmployeesCommandHandler(Context);
        var request = new BulkDeleteEmployeesRequest{EmployeeIds = new List<Guid> { employeeId }};

        // Act
        var result = await handler.Handle(new DeleteEmployeesCommand(request), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsError);
        var employee = await Context.Employees.FindAsync(employeeId);
        Assert.Null(employee);
    }
}