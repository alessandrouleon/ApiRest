using APIRest.Application.DTOs.Requests;
using FluentValidation;

namespace APIRest.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Identifier (email or username) is required.")
            .MaximumLength(255).WithMessage("Identifier must not exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
    }
}
