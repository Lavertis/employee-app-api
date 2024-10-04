using EmployeeApp.API.Dto.Result;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Commands.Employees;

public class DeleteEmployeeCommand : IRequest<HttpResult<Unit>>
{
    public Guid Id { get; }

    public DeleteEmployeeCommand(Guid id)
    {
        Id = id;
    }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, HttpResult<Unit>>
{
    private readonly EmployeeDbContext _context;

    public DeleteEmployeeCommandHandler(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HttpResult<Unit>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = new HttpResult<Unit>();
        var employee = await _context.Employees
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            return result
                .WithError(new Error { Message = "Employee not found" })
                .WithStatusCode(StatusCodes.Status404NotFound);
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return result.WithStatusCode(StatusCodes.Status204NoContent);
    }
}