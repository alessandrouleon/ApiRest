using APIRest.Application.DTOs.Requests;
using APIRest.Domain.Enums;
using FluentValidation;

namespace APIRest.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9_.]+$").WithMessage("Username can only contain letters, numbers, underscores, and dots.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive is required.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Role must be a valid value.");

        When(x => x.Password is not null, () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        });
    }
}
