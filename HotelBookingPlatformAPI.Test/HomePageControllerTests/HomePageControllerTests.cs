using FluentAssertions;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.HomePage;
namespace HotelBookingPlatformApplication.Test.HomePageControllerTests;
public class HomePageControllerTests
{
    private readonly HomePageController _sut;
    private readonly Mock<ICityService> _mockCityService;
    private readonly Mock<IHotelService> _mockHotelService;
    private readonly Mock<IRoomService> _mockRoomService;
    private readonly Fixture _fixture;

    public HomePageControllerTests()
    {
        _fixture = new Fixture();
        _mockCityService = new Mock<ICityService>();
        _mockHotelService = new Mock<IHotelService>();
        _mockRoomService = new Mock<IRoomService>();
        _sut = new HomePageController(_mockCityService.Object, _mockHotelService.Object, _mockRoomService.Object);
    }
    [Fact]
    public async Task GetTrendingDestinations_ShouldReturnTopCities_WhenCitiesExist()
    {
        // Arrange
        var topCities = _fixture.CreateMany<CityResponseDto>(5).ToList();
        _mockCityService.Setup(s => s.GetTopVisitedCitiesAsync(5))
                        .ReturnsAsync(topCities);

        // Act
        var result = await _sut.GetTrendingDestinations();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<CityResponseDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(topCities);
    }

    [Fact]
    public async Task GetTrendingDestinations_ShouldReturnNotFound_WhenNoCitiesExist()
    {
        // Arrange
        _mockCityService.Setup(s => s.GetTopVisitedCitiesAsync(5))
                        .ReturnsAsync(new List<CityResponseDto>());

        // Act
        var result = await _sut.GetTrendingDestinations();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<CityResponseDto>>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().BeEquivalentTo(new { message = "No cities found." });
    }

    [Fact]
    public async Task Search_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var searchRequest = _fixture.Create<SearchRequestDto>();
        var exceptionMessage = "Database connection failed";
        _mockHotelService.Setup(s => s.SearchHotelsAsync(searchRequest))
                         .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _sut.Search(searchRequest);

        // Assert
        var actionResult = Assert.IsType<ActionResult<SearchResultsDto>>(result);
        var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);

        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().BeEquivalentTo(new
        {
            message = "An error occurred while processing your request.",
            error = exceptionMessage
        });
    }
}
