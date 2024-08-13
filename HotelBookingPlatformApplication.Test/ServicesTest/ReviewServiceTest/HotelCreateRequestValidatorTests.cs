namespace HotelBookingPlatformApplication.Test.ServicesTest.ReviewServiceTest;
public class HotelCreateRequestValidatorTests
{
    private readonly HotelCreateRequestValidator _validator;

    public HotelCreateRequestValidatorTests()
    {
        _validator = new HotelCreateRequestValidator();
    }

    [Theory]
    [InlineData("Sunrise Hotel", "+123456789", 5, 1, null)] // Valid case
    [InlineData("Sunrise Hotel", "+123456789", 6, 1, "StarRating must be between 1 and 5")]
    [InlineData("Sunrise Hotel", "", 3, 1, "PhoneNumber is required")]
    [InlineData("", "+123456789", 3, 1, "Name is required")]
    [InlineData("Sunrise Hotel", "+123456789", 3, -1, "OwnerID must be a positive integer")]
    public void Validate_ShouldReturnExpectedResults(string name, string phoneNumber, int starRating, int ownerId, string expectedErrorMessage)
    {
        // Arrange
        var request = new HotelCreateRequest
        {
            Name = name,
            PhoneNumber = phoneNumber,
            StarRating = starRating,
            OwnerID = ownerId
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        if (expectedErrorMessage == null)
        {
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
            result.ShouldNotHaveValidationErrorFor(x => x.StarRating);
            result.ShouldNotHaveValidationErrorFor(x => x.OwnerID);
        }
        else
        {
            switch (expectedErrorMessage)
            {
                case "Name is required":
                    result.ShouldHaveValidationErrorFor(x => x.Name)
                          .WithErrorMessage(expectedErrorMessage);
                    break;
                case "PhoneNumber is required":
                    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                          .WithErrorMessage(expectedErrorMessage);
                    break;
                case "StarRating must be between 1 and 5":
                    result.ShouldHaveValidationErrorFor(x => x.StarRating)
                          .WithErrorMessage(expectedErrorMessage);
                    break;
                case "OwnerID must be a positive integer":
                    result.ShouldHaveValidationErrorFor(x => x.OwnerID)
                          .WithErrorMessage(expectedErrorMessage);
                    break;
            }
        }
    }
}
