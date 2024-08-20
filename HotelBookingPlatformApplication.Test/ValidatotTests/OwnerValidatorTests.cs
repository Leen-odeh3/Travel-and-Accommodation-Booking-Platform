using FluentValidation.TestHelper;
namespace HotelBookingPlatformApplication.Test.ValidatorTests;
public class OwnerValidatorTests
{
    private readonly OwnerValidator _validator;
    public OwnerValidatorTests()
    {
        _validator = new OwnerValidator();
    }

    [Theory]
    [InlineData("", "First name is required")]
    [InlineData("123", "First name can only contain letters")]
    [InlineData("John", null)]
    public void Should_Validate_FirstName(string firstName, string expectedErrorMessage)
    {
        // Arrange
        var model = new OwnerCreateDto { FirstName = firstName };
        var result = _validator.TestValidate(model);

        if (expectedErrorMessage is not null)
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage(expectedErrorMessage);
        else

            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Theory]
    [InlineData("", "Last name is required")]
    [InlineData("123", "Last name can only contain letters")]
    [InlineData("Doe", null)]
    public void Should_Validate_LastName(string lastName, string expectedErrorMessage)
    {
        var model = new OwnerCreateDto { LastName = lastName };
        var result = _validator.TestValidate(model);

        // Assert
        if (expectedErrorMessage is not null)
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage(expectedErrorMessage);
        else
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }

    [Theory]
    [InlineData("", "Email is required")]
    [InlineData("leenodeh287@gmail.com", null)]
    public void Should_Validate_Email(string email, string expectedErrorMessage)
    {
        var model = new OwnerCreateDto { Email = email };
        var result = _validator.TestValidate(model);

        if (expectedErrorMessage is not null)
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage(expectedErrorMessage);
        else
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
