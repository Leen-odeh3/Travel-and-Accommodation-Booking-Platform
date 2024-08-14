namespace HotelBookingPlatformAPI.Test.HotelAPITest;
public class HotelControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IHotelService> _hotelServiceMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly HotelController _controller;

    public HotelControllerTest()
    {
        _fixture = new Fixture();
        _hotelServiceMock = new Mock<IHotelService>();
        _imageServiceMock = new Mock<IImageService>();
        _controller = new HotelController(_hotelServiceMock.Object, _imageServiceMock.Object);
    }

    [Fact]
    public async Task GetHotels_ShouldReturnHotels_WhenHotelsExist()
    {
        // Arrange
        var hotels = _fixture.CreateMany<HotelResponseDto>(2).ToList();
        _hotelServiceMock.Setup(service => service.GetHotels(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(hotels);

        var result = await _controller.GetHotels("Skyline Suites", "An upscale hotel with stunning skyline views, an on-site restaurant, and a full-service spa.", 10, 1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<HotelResponseDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<List<HotelResponseDto>>(okResult.Value);
        returnValue.Should().BeEquivalentTo(hotels);
    }

    [Fact]
    public async Task GetHotel_ShouldReturnHotel_WhenHotelExists()
    {
        // Arrange
        var hotel = _fixture.Create<HotelResponseDto>();
        _hotelServiceMock.Setup(service => service.GetHotel(It.IsAny<int>()))
                         .ReturnsAsync(hotel);

        // Act
        var result = await _controller.GetHotel(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<HotelResponseDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<HotelResponseDto>(okResult.Value);
        returnValue.Should().BeEquivalentTo(hotel);
    }    
   
    [Fact]
    public async Task UpdateHotel_ShouldReturnUpdatedHotel_WhenHotelIsUpdated()
    {
        // Arrange
        var id = 1;
        var request = _fixture.Create<HotelResponseDto>();
        var updatedHotel = _fixture.Create<HotelResponseDto>();
        _hotelServiceMock.Setup(service => service.UpdateHotelAsync(It.IsAny<int>(), It.IsAny<HotelResponseDto>()))
                         .ReturnsAsync(updatedHotel);

        // Act
        var result = await _controller.UpdateHotel(id, request);

        var actionResult = Assert.IsType<ActionResult<HotelResponseDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<HotelResponseDto>(okResult.Value);
        returnValue.Should().BeEquivalentTo(updatedHotel);
    }

    [Fact]
    public async Task SearchHotel_ShouldReturnHotels_WhenHotelsMatchCriteria()
    {
        // Arrange
        var hotels = _fixture.CreateMany<HotelResponseDto>(2).ToList();
        _hotelServiceMock.Setup(service => service.SearchHotel(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(hotels);

        // Act
        var result = await _controller.SearchHotel("TestName", "TestDesc", 10, 1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<HotelResponseDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsType<List<HotelResponseDto>>(okResult.Value);
        returnValue.Should().BeEquivalentTo(hotels);
    }
}


