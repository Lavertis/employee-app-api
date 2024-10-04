using EmployeeApp.API.Dto.Common;
using EmployeeApp.API.Dto.Employee;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Infrastructure.Database;
using FluentValidation;
using MediatR;

namespace EmployeeApp.API.CQRS.Commands.Employees;

public class CreateEmployeeCommand : IRequest<HttpResult<IdResponse<Guid>>>
{
    public CreateEmployeeRequest Request { get; }

    public CreateEmployeeCommand(CreateEmployeeRequest request)
    {
        Request = request;
    }
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, HttpResult<IdResponse<Guid>>>
{
    private readonly EmployeeDbContext _context;
    private readonly IValidator<CreateEmployeeRequest> _validator;

    public CreateEmployeeCommandHandler(EmployeeDbContext context, IValidator<CreateEmployeeRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<HttpResult<IdResponse<Guid>>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result = new HttpResult<IdResponse<Guid>>();
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return result.WithValidationErrors(validationResult.Errors);

        var employee = CreateEmployee(command.Request);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new IdResponse<Guid>(employee.Id);
        return result.WithValue(response);
    }

    private static Employee CreateEmployee(CreateEmployeeRequest request)
    {
        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age,
            SexId = request.SexId
        };
        return employee;
    }
}