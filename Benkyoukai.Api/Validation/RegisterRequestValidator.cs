using Benkyoukai.Contracts.Authentication;
using FluentValidation;

namespace Benkyoukai.Api.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("Username must be between 3 and 20 characters long and can only contain letters and numbers.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .WithMessage("Password must be between 6 and 20 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Passwords must match.");
    }
}
