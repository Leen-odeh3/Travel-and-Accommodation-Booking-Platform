using FluentValidation;
using HotelBookingPlatform.Domain.DTOs.Register;

namespace HotelBookingPlatform.Application.Validator;
public class AddRoleAdminValidator:AbstractValidator<AdminAssignmentRequestDto>
{
    public AddRoleAdminValidator()
    {
        ApplyValidationRoles();
    }
    public void ApplyValidationRoles()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
