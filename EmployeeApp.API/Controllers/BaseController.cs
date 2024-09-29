using EmployeeApp.API.Dto.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.API.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;

    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected ActionResult<TValue> CreateResponse<TValue>(HttpResult<TValue> result)
    {
        return result.StatusCode switch
        {
            >= 200 and < 300 => StatusCode(result.StatusCode, result.Value),
            _ when result.IsError => StatusCode(result.StatusCode, new {Error = result.Error?.Message}),
            _ when result.HasValidationErrors => StatusCode(result.StatusCode, new {Errors = result.ValidationErrors}),
            _ => throw new Exception("Failed to created response")
        };
    }
}