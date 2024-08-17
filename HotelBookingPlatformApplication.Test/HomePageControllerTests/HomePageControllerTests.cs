using HotelBookingPlatform.API.Controllers;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.HomePage;
using Microsoft.AspNetCore.Mvc;

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
        var result = await _sut.GetTrendingDestinations() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(topCities);
    }

    [Fact]
    public async Task GetTrendingDestinations_ShouldReturnNotFound_WhenNoCitiesExist()
    {
        // Arrange
        _mockCityService.Setup(s => s.GetTopVisitedCitiesAsync(5))
                        .ReturnsAsync(new List<CityResponseDto>());

        // Act
        var result = await _sut.GetTrendingDestinations() as NotFoundObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().BeEquivalentTo(new { message = "No cities found." });
    }

    [Fact]
    public async Task Search_ShouldReturnSearchResults_WhenHotelsExist()
    {
        // Arrange
        var searchRequest = _fixture.Create<SearchRequestDto>();
        var searchResults = _fixture.Create<SearchResultsDto>();
        _mockHotelService.Setup(s => s.SearchHotelsAsync(searchRequest))
                         .ReturnsAsync(searchResults);

        // Act
        var result = await _sut.Search(searchRequest) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(searchResults);
    }

    [Fact]
    public async Task Search_ShouldReturnNotFound_WhenNoHotelsMatch()
    {
        // Arrange
        var searchRequest = _fixture.Create<SearchRequestDto>();
        var searchResults = new SearchResultsDto { Hotels = new List<HotelResponseDto>() };
        _mockHotelService.Setup(s => s.SearchHotelsAsync(searchRequest))
                         .ReturnsAsync(searchResults);

        // Act
        var result = await _sut.Search(searchRequest) as NotFoundObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().BeEquivalentTo(new { message = "No hotels found matching the search criteria." });
    }

    [Fact]
    public async Task Search_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var searchRequest = _fixture.Create<SearchRequestDto>();
        _mockHotelService.Setup(s => s.SearchHotelsAsync(searchRequest))
                         .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _sut.Search(searchRequest) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(500);
        result.Value.Should().BeEquivalentTo(new { message = "An error occurred while processing your request.", error = "Test exception" });
    }
}