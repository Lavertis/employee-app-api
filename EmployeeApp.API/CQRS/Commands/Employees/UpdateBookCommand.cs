using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.Domain.Entities;
using EmployeeApp.Infrastructure.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Commands.Employees;

public class UpdateEmployeeCommand : IRequest<HttpResult<Unit>>
{
    public Guid EmployeeId { get; }
    public UpdateEmployeeRequest Request { get; }

    public UpdateEmployeeCommand(Guid employeeId, UpdateEmployeeRequest request)
    {
        EmployeeId = employeeId;
        Request = request;
    }
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, HttpResult<Unit>>
{
    private readonly EmployeeDbContext _context;
    private readonly IValidator<UpdateEmployeeRequest> _validator;

    public UpdateEmployeeCommandHandler(EmployeeDbContext context, IValidator<UpdateEmployeeRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<HttpResult<Unit>> Handle(UpdateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<Unit>();
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
            return result.WithValidationErrors(validationResult.Errors);

        var employee = await _context.Employees
            .Include(e => e.Sex)
            .FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

        if (employee == null)
        {
            return result
                .WithError(new Error { Message = "Employee not found" })
                .WithStatusCode(StatusCodes.Status404NotFound);
        }

        UpdateEmployee(employee, command.Request);
        await _context.SaveChangesAsync(cancellationToken);
        return result.WithStatusCode(StatusCodes.Status204NoContent);
    }

    private static void UpdateEmployee(Employee employee, UpdateEmployeeRequest request)
    {
        if (request.FirstName != null)
            employee.FirstName = request.FirstName;
        if (request.LastName != null)
            employee.LastName = request.LastName;
        if (request.Age != null)
            employee.Age = request.Age.Value;
        if (request.SexId != null)
            employee.SexId = request.SexId.Value;
    }
}