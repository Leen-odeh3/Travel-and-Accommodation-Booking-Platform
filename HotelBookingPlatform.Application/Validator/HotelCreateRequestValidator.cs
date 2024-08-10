namespace HotelBookingPlatform.Application.Validator;
public class HotelCreateRequestValidator : AbstractValidator<HotelCreateRequest>
{
    public HotelCreateRequestValidator()
    {
        ApplyValidationRoles();
    }
    public void ApplyValidationRoles()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.StarRating)
            .InclusiveBetween(1, 5).WithMessage("StarRating must be between 1 and 5");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required");

        RuleFor(x => x.OwnerID)
            .GreaterThan(0).WithMessage("OwnerID must be a positive integer");
    }
}
