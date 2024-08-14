namespace HotelBookingPlatformAPI.Test.HotelAPITest;
public class HotelControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IHotelService> _mockHotelService;
    private readonly Mock<IImageService> _mockImageService;
    private readonly HotelController _controller;

    public HotelControllerTests()
    {
        _fixture = new Fixture();
        _mockHotelService = new Mock<IHotelService>();
        _mockImageService = new Mock<IImageService>();
    }

    [Fact]
    public async Task CreateHotel_ShouldReturnCreatedHotel()
    {
        // Arrange
        var hotelCreateRequest = _fixture.Create<HotelCreateRequest>();
        var createdHotel = _fixture.Create<HotelResponseDto>();

        _mockHotelService
            .Setup(service => service.CreateHotel(hotelCreateRequest))
            .ReturnsAsync(createdHotel);

        // Act
        var result = await _controller.CreateHotel(hotelCreateRequest);

        // Assert
        var actionResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<HotelResponseDto>(actionResult.Value);
        Assert.Equal(createdHotel, response);
    }
}