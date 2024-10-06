using EmployeeApp.API.Dto.Employee.Requests;
using EmployeeApp.Infrastructure.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeApp.API.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator(EmployeeDbContext context)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .MinimumLength(1)
            .MaximumLength(255)
            .When(x => !x.FirstName.IsNullOrEmpty());
        
        RuleFor(x => x.LastName)
            .MinimumLength(1)
            .MaximumLength(255)
            .When(x => !x.LastName.IsNullOrEmpty());

        RuleFor(x => x.Age)
            .NotEmpty()
            .GreaterThanOrEqualTo(18)
            .LessThanOrEqualTo(100)
            .When(x => x.Age != null);
        
        RuleFor(x => x.SexId)
            .MustAsync(async (publisherId, cancellationToken) =>
                await context.Sexes.AnyAsync(x => x.Id == publisherId, cancellationToken)
            )
            .When(x => x.SexId != null)
            .WithMessage("Sex not found");
    }
}