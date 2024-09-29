using EmployeeApp.API.Dto.Common;
using EmployeeApp.API.Dto.Employee;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Queries.Employees;

public class GetEmployeesQuery : IRequest<HttpResult<PagedResponse<EmployeeListItemResponse>>>
{
    public GetEmployeesQueryParams QueryParams { get; }

    public GetEmployeesQuery(GetEmployeesQueryParams queryParams)
    {
        QueryParams = queryParams;
    }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, HttpResult<PagedResponse<EmployeeListItemResponse>>>
{
    private readonly EmployeeDbContext _context;

    public GetEmployeesQueryHandler(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HttpResult<PagedResponse<EmployeeListItemResponse>>> Handle(GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<PagedResponse<EmployeeListItemResponse>>();
        var query = _context.Employees
            .Include(e => e.Sex);

        var employees = await query
            .Skip((request.QueryParams.Page - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .Select(e => new EmployeeListItemResponse
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Age = e.Age,
                Sex = e.Sex.Name
            })
            .ToListAsync(cancellationToken);

        var pagedResponse = new PagedResponse<EmployeeListItemResponse>
        {
            PageNumber = request.QueryParams.Page,
            PageSize = request.QueryParams.PageSize,
            Records = employees,
            TotalRecords = await query.CountAsync(cancellationToken)
        };

        return result.WithValue(pagedResponse);
    }
}