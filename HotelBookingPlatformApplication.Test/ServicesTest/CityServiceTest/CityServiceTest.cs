using HotelBookingPlatform.Domain.DTOs.Hotel;
using Moq;

namespace HotelBookingPlatformApplication.Test.ServicesTest.CityServiceTest;
public class CityServiceTest
{
    private readonly Mock<IUnitOfWork<City>> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CityService _cityService;

    public CityServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<City>>();
        _mockMapper = new Mock<IMapper>();
        _cityService = new CityService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddHotelToCityAsync_ShouldThrowKeyNotFoundException_WhenCityDoesNotExist()
    {
        // Arrange
        int cityId = 1;
        var hotelRequest = DataSetup.CreateSampleHotelCreateRequest();
        _mockUnitOfWork.Setup(u => u.CityRepository.GetByIdAsync(cityId))
            .ReturnsAsync((City)null); 

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException>(() =>
            _cityService.AddHotelToCityAsync(cityId, hotelRequest));
        Assert.Equal("City not found.", exception.Message);
    }


}
