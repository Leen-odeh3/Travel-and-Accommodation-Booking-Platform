using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatformAPI.Test.CityAPITest;
public class CityControllerTest
{
    private readonly Mock<ICityService> _mockCityService;
    private readonly Mock<IResponseHandler> _mockResponseHandler;
    private readonly CityController _controller;
    private readonly Fixture _fixture;
    private readonly ICityService _cityService;
    private readonly Mock<IUnitOfWork<City>> _mockUnitOfWork;
    public CityControllerTest()
    {
        _mockCityService = new Mock<ICityService>();
        _mockResponseHandler = new Mock<IResponseHandler>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddCity_ReturnsCreatedResponse_WhenCityIsAddedSuccessfully()
    {
        // Arrange
        var request = _fixture.Create<CityCreateRequest>();
        var cityResponse = _fixture.Create<CityResponseDto>();
        _mockCityService.Setup(x => x.AddCityAsync(request)).ReturnsAsync(cityResponse);
        _mockResponseHandler.Setup(x => x.Created(cityResponse, "Owner created successfully."))
                            .Returns(new ObjectResult(cityResponse) { StatusCode = StatusCodes.Status201Created });

        // Act
        var result = await _controller.AddCity(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        Assert.Equal(cityResponse, objectResult.Value);
    }

}
