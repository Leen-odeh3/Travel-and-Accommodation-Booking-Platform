namespace HotelBookingPlatformApplication.Test.AuthServiceTest;
public class RegisterUserValidatorTest
{
    private readonly RegisterUserValidator _validator;

    public RegisterUserValidatorTest()
    {
        _validator = new RegisterUserValidator();
    }

    [Theory]
    [InlineData("", "odeh", "leenodeh287@gmail.com", "Password123!", "FirstName is required")]
    [InlineData("leen", "", "leenodeh287@gmail.com", "Password123!", "LastName is required")]
    [InlineData("leen", "odeh", "", "Password123!", "Email is required")]
    [InlineData("leen", "odeh", "leenodeh287@gmail.com", "", "Password is required")]
    [InlineData("leen", "odeh", "leenodeh287@gmail.com", "short", "Password must be at least 10 characters long")]
    [InlineData("leen", "odeh", "leenodeh287@gmail.com", "NoDigits!", "Password must contain at least one digit")]
    [InlineData("leen", "odeh", "leenodeh287@gmail.com", "NoSpecialChar123", "Password must contain at least one special character")]
    public void Validate_RegisterModel_InvalidData_ReturnsExpectedError(
     string firstName,
     string lastName,
     string email,
     string password,
     string expectedError)
    {
        // Arrange
        var model = new RegisterModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        // Act
        var result = _validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        errorMessages.Should().Contain(expectedError);
    }

}