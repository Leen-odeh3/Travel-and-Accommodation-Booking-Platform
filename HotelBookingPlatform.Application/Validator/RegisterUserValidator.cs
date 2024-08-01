using FluentValidation;
using HotelBookingPlatform.Domain.Helpers;
namespace HotelBookingPlatform.Application.Validator;
public class RegisterUserValidator : AbstractValidator<RegisterModel>
{
    public RegisterUserValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required")
            .Matches("^[a-zA-Z]").WithMessage("FirstName can only contain letters");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("LastName is required")
           .Matches("^[a-zA-Z]").WithMessage("LastName can only contain letters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]").WithMessage("Password must contain at least one special character");
    }
}
