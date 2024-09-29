using EmployeeApp.API.Dto.Result;
using EmployeeApp.API.Dto.Sex;
using EmployeeApp.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.CQRS.Queries.Sexes;

public class GetSexesQuery : IRequest<HttpResult<IEnumerable<SexResponse>>>;

public class GetSexesQueryHandler : IRequestHandler<GetSexesQuery, HttpResult<IEnumerable<SexResponse>>>
{
    private readonly EmployeeDbContext _context;

    public GetSexesQueryHandler(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HttpResult<IEnumerable<SexResponse>>> Handle(GetSexesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new HttpResult<IEnumerable<SexResponse>>();
        var sexes = await _context.Sexes
            .Select(s => new SexResponse { Id = s.Id, Name = s.Name })
            .ToListAsync(cancellationToken);
        return result.WithValue(sexes);
    }
}