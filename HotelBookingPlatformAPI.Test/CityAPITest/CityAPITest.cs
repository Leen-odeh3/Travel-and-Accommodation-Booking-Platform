namespace HotelBookingPlatformAPI.Test.CityAPITest;
public class CityControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<ICityService> _cityServiceMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly CityController _controller;

    public CityControllerTest()
    {
        _fixture = new Fixture();
        _cityServiceMock = new Mock<ICityService>();
        _imageServiceMock = new Mock<IImageService>();
        _controller = new CityController(_cityServiceMock.Object, _imageServiceMock.Object);
    }

    [Fact]
    public async Task AddCity_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var cityCreateRequest = _fixture.Create<CityCreateRequest>();
        var cityResponse = _fixture.Create<CityResponseDto>();

        _cityServiceMock
            .Setup(service => service.AddCityAsync(cityCreateRequest))
            .ReturnsAsync(cityResponse);

        // Act
        var result = await _controller.AddCity(cityCreateRequest);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.ActionName.Should().Be(nameof(CityController.GetCity));
        createdResult.RouteValues["id"].Should().Be(cityResponse.CityID);
        createdResult.Value.Should().BeEquivalentTo(cityResponse);
    }

    [Fact]
    public async Task DeleteCity_SuccessfulDeletion_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();
        _cityServiceMock
            .Setup(service => service.DeleteAsync(cityId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCity(cityId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(new { Message = "City deleted successfully." });
    }

  
    [Fact]
    public async Task AddHotelToCity_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();
        var hotelCreateRequest = _fixture.Create<HotelCreateRequest>();

        _cityServiceMock
            .Setup(service => service.AddHotelToCityAsync(cityId, hotelCreateRequest))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddHotelToCity(cityId, hotelCreateRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(new { Message = "Hotel added to city successfully." });
    }

    [Fact]
    public async Task DeleteHotelFromCity_SuccessfulDeletion_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();
        var hotelId = _fixture.Create<int>();

        _cityServiceMock
            .Setup(service => service.DeleteHotelFromCityAsync(cityId, hotelId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteHotelFromCity(cityId, hotelId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(new { Message = "Hotel removed from city successfully." });
    }

    [Fact]
    public async Task UploadImages_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();
        var files = new List<IFormFile>();

        _imageServiceMock
            .Setup(service => service.UploadImagesAsync("City", cityId, files))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UploadImages(cityId, files);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().Be("Images uploaded successfully.");
    }


    [Fact]
    public async Task DeleteImage_SuccessfulDeletion_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();
        var imageId = _fixture.Create<int>();

        _imageServiceMock
            .Setup(service => service.DeleteImageAsync("City", cityId, imageId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteImage(cityId, imageId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().Be("Image deleted successfully.");
    }

    [Fact]
    public async Task DeleteAllImages_SuccessfulDeletion_ReturnsOkResult()
    {
        // Arrange
        var cityId = _fixture.Create<int>();

        _imageServiceMock
            .Setup(service => service.DeleteAllImagesAsync("City", cityId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAllImages(cityId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().Be("All images deleted successfully.");
    }
}