using EmployeeApp.API.CQRS.Commands.Books;
using EmployeeApp.API.CQRS.Queries.Employees;
using EmployeeApp.API.Dto.Common;
using EmployeeApp.API.Dto.Employee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : BaseController
{
    public EmployeeController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<EmployeeListItemResponse>>> GetEmployeesAsync(
        [FromQuery] GetEmployeesQueryParams query)
        => CreateResponse(await Mediator.Send(new GetEmployeesQuery(query)));

    [HttpGet("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailsResponse>> GetEmployeeAsync(Guid employeeId)
        => CreateResponse(await Mediator.Send(new GetEmployeeQuery(employeeId)));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateEmployeesAsync(CreateEmployeeRequest request)
        => CreateResponse(await Mediator.Send(new CreateEmployeeCommand(request)));

    [HttpPatch("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Unit>> UpdateEmployeeByIdAsync(Guid employeeId, UpdateEmployeeRequest request)
        => CreateResponse(await Mediator.Send(new UpdateEmployeeCommand(employeeId, request)));

    [HttpDelete("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<Unit>> DeleteEmployeeAsync(Guid employeeId)
        => CreateResponse(await Mediator.Send(new DeleteEmployeeCommand(employeeId)));
}