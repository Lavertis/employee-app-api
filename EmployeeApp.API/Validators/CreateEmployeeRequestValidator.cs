using EmployeeApp.API.Dto.Employee;
using EmployeeApp.Infrastructure.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator(EmployeeDbContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(255);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(255);

        RuleFor(x => x.Age)
            .NotEmpty()
            .GreaterThanOrEqualTo(18)
            .LessThanOrEqualTo(100);
        
        RuleFor(x => x.SexId)
            .NotEmpty()
            .MustAsync(async (sexId, cancellationToken) =>
                await context.Sexes.AnyAsync(x => x.Id == sexId, cancellationToken)
            )
            .WithMessage("Sex not found");
    }
}