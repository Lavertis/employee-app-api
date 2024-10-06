using AutoMapper;
using EmployeeApp.API.Dto.Employee.Responses;
using EmployeeApp.API.Dto.Result;
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
    private readonly IMapper _mapper;

    public GetEmployeeQueryHandler(EmployeeDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HttpResult<EmployeeDetailsResponse>> Handle(GetEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<EmployeeDetailsResponse>();
        var employee = await _context.Employees
            .Include(e => e.Sex)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            return result
                .WithError(new Error { Message = "Employee not found" })
                .WithStatusCode(StatusCodes.Status404NotFound);
        }
        
        var employeeResponse = _mapper.Map<EmployeeDetailsResponse>(employee);
        return result.WithValue(employeeResponse);
    }
}