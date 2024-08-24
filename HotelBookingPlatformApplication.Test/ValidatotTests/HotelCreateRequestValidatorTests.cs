using FluentValidation.TestHelper;
namespace HotelBookingPlatformApplication.Test.ValidatotTests;
public class HotelCreateRequestValidatorTests
{
    private readonly HotelCreateRequestValidator _validator;
    public HotelCreateRequestValidatorTests()
    {
        _validator = new HotelCreateRequestValidator();
    }

    [Theory]
    [InlineData(0, "StarRating must be between 1 and 5")]
    [InlineData(6, "StarRating must be between 1 and 5")]
    [InlineData(-1, "StarRating must be between 1 and 5")]
    public void Should_Have_Error_When_StarRating_Is_Out_Of_Range(int starRating, string expectedErrorMessage)
    {
        // Arrange
        var model = new HotelCreateRequest { StarRating = starRating };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StarRating)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new HotelCreateRequest { Name = "" };
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Is_Empty()
    {
        // Arrange
        var model = new HotelCreateRequest { PhoneNumber = "" };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("PhoneNumber is required");
    }

    [Theory]
    [InlineData(0, "OwnerID must be a positive integer")]
    [InlineData(-1, "OwnerID must be a positive integer")]
    public void Should_Have_Error_When_OwnerID_Is_Not_Positive(int ownerId, string expectedErrorMessage)
    {
        // Arrange
        var model = new HotelCreateRequest { OwnerID = ownerId };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.OwnerID)
            .WithErrorMessage(expectedErrorMessage);
    }


    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        // Arrange
        var model = new HotelCreateRequest
        {
            Name = "The Ritz-Carlton",
            StarRating = 3,
            PhoneNumber = "123456789",
            OwnerID = 1
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
