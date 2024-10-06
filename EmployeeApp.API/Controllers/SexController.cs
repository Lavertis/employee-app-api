using EmployeeApp.API.CQRS.Queries.Sexes;
using EmployeeApp.API.Dto.Sex;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.API.Controllers;

[ApiController]
[Route("api/sexes")]
public class SexController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SexResponse>>> GetSexesAsync()
        => CreateResponse(await Mediator.Send(new GetSexesQuery()));
}