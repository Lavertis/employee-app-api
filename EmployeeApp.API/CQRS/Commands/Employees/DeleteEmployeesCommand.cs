using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Commands.Employees;

public class DeleteEmployeesCommand : IRequest<HttpResult<Unit>>
{
    public IList<Guid> Ids { get; }

    public DeleteEmployeesCommand(BulkDeleteEmployeesRequest request)
    {
        Ids = request.EmployeeIds;
    }
}

public class DeleteEmployeesCommandHandler : IRequestHandler<DeleteEmployeesCommand, HttpResult<Unit>>
{
    private readonly EmployeeDbContext _context;

    public DeleteEmployeesCommandHandler(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HttpResult<Unit>> Handle(DeleteEmployeesCommand request, CancellationToken cancellationToken)
    {
        var result = await ValidateRequest(request, cancellationToken);
        if (result.IsError)
            return result;

        _context.Employees.RemoveRange(_context.Employees.Where(b => request.Ids.Contains(b.Id)));
        await _context.SaveChangesAsync(cancellationToken);
        return result.WithStatusCode(StatusCodes.Status204NoContent);
    }

    private async Task<HttpResult<Unit>> ValidateRequest(DeleteEmployeesCommand request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<Unit>();
        var areIdsUnique = request.Ids.Distinct().Count() == request.Ids.Count;
        if (!areIdsUnique)
        {
            return result
                .WithError(new Error { Message = "Request contains duplicate Ids" })
                .WithStatusCode(StatusCodes.Status400BadRequest);
        }
        
        var foundEmployeeIds = await _context.Employees
            .Where(b => request.Ids.Contains(b.Id))
            .Select(b => b.Id)
            .ToListAsync(cancellationToken);

        var notFoundEmployeeIds = request.Ids.Except(foundEmployeeIds).ToList();
        if (notFoundEmployeeIds.Count != 0)
        {
            return result
                .WithError(new Error
                    { Message = $"Employees with Ids {string.Join(", ", notFoundEmployeeIds)} not found" })
                .WithStatusCode(StatusCodes.Status404NotFound);
        }

        return result.WithStatusCode(StatusCodes.Status204NoContent);
    }
}