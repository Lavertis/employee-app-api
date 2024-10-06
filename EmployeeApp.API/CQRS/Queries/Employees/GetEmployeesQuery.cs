using AutoMapper;
using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.API.Dto.Employee.Responses;
using EmployeeApp.API.Dto.Result;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Queries.Employees;

public class GetEmployeesQuery : IRequest<HttpResult<IEnumerable<EmployeeListItemResponse>>>
{
    public GetEmployeesQueryParams QueryParams { get; }

    public GetEmployeesQuery(GetEmployeesQueryParams queryParams)
    {
        QueryParams = queryParams;
    }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, HttpResult<IEnumerable<EmployeeListItemResponse>>>
{
    private readonly EmployeeDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(EmployeeDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HttpResult<IEnumerable<EmployeeListItemResponse>>> Handle(GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<IEnumerable<EmployeeListItemResponse>>();
        var query = _context.Employees
            .Include(e => e.Sex);

        var employees = await query
            .OrderBy(e => e.Id)
            .Skip((request.QueryParams.Page - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .Select(e => _mapper.Map<EmployeeListItemResponse>(e))
            .ToListAsync(cancellationToken);

        var totalRecords = await query.CountAsync(cancellationToken);
        return result
            .WithPagination(request.QueryParams.Page, request.QueryParams.PageSize, totalRecords)
            .WithValue(employees);
    }
}