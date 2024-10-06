using EmployeeApp.API.CQRS.Commands.Employees;
using EmployeeApp.API.CQRS.Queries.Employees;
using EmployeeApp.API.Dto.Common;
using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.API.Dto.Employee.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmployeeListItemResponse>>> GetEmployeesAsync(
        [FromQuery] GetEmployeesQueryParams query)
        => CreateResponse(await Mediator.Send(new GetEmployeesQuery(query)));

    [HttpGet("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailsResponse>> GetEmployeeAsync([FromRoute] Guid employeeId)
        => CreateResponse(await Mediator.Send(new GetEmployeeQuery(employeeId)));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateEmployeesAsync([FromBody] CreateEmployeeRequest request)
        => CreateResponse(await Mediator.Send(new CreateEmployeeCommand(request)));

    [HttpPatch("{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Unit>> UpdateEmployeeByIdAsync([FromRoute] Guid employeeId, [FromBody] UpdateEmployeeRequest request)
        => CreateResponse(await Mediator.Send(new UpdateEmployeeCommand(employeeId, request)));
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<Unit>> DeleteEmployeesAsync([FromBody] BulkDeleteEmployeesRequest request)
        => CreateResponse(await Mediator.Send(new DeleteEmployeesCommand(request)));
}