namespace HotelBookingPlatform.Application.Validator;
public class OwnerValidator : AbstractValidator<OwnerCreateDto>
{
    public OwnerValidator()
    {
        ApplyValidationRoles();
    }
    public void ApplyValidationRoles()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Matches("^[a-zA-Z\\u0900-\\u097F]+$").WithMessage("First name can only contain letters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Matches("^[a-zA-Z\\u0900-\\u097F]+$").WithMessage("Last name can only contain letters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .WithMessage("Phone number must be a valid Indian number with 10 digits, optionally preceded by +");
    }
}