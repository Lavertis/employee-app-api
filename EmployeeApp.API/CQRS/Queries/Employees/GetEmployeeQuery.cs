using EmployeeApp.API.Dto.Employee;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.API.Dto.Sex;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Queries.Employees;

public class GetEmployeeQuery : IRequest<HttpResult<EmployeeDetailsResponse>>
{
    public Guid Id { get; }

    public GetEmployeeQuery(Guid id)
    {
        Id = id;
    }
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, HttpResult<EmployeeDetailsResponse>>
{
    private readonly EmployeeDbContext _context;

    public GetEmployeeQueryHandler(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HttpResult<EmployeeDetailsResponse>> Handle(GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<EmployeeDetailsResponse>();
        var book = await _context.Employees
            .Include(e => e.Sex)
            .Select(e => new EmployeeDetailsResponse
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Age = e.Age,
                Sex = new SexResponse{Id = e.Sex.Id, Name = e.Sex.Name}
            })
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (book == null)
        {
            return result
                .WithError(new Error { Message = "Book not found" })
                .WithStatusCode(StatusCodes.Status404NotFound);
        }

        return result.WithValue(book);
    }
}