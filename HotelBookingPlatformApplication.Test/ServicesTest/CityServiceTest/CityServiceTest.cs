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
    public async Task AddCityAsync_Should_Add_City_And_Return_CityResponseDto()
    {
        var cityCreateRequest = TestData.CityCreateRequest;
        var city = TestData.City;
        var cityResponseDto = TestData.CityResponseDto;

        _mockMapper.Setup(m => m.Map<City>(cityCreateRequest)).Returns(city);
        _mockMapper.Setup(m => m.Map<CityResponseDto>(city)).Returns(cityResponseDto);
        _mockUnitOfWork.Setup(u => u.CityRepository.CreateAsync(city)).ReturnsAsync(city);

        var result = await _cityService.AddCityAsync(cityCreateRequest);

        Assert.Equal(cityResponseDto.CityID, result.CityID);
        Assert.Equal(cityResponseDto.Name, result.Name);

        _mockMapper.Verify(m => m.Map<City>(cityCreateRequest), Times.Once);
        _mockMapper.Verify(m => m.Map<CityResponseDto>(city), Times.Once);
        _mockUnitOfWork.Verify(u => u.CityRepository.CreateAsync(city), Times.Once);
    }

    [Fact]
    public async Task GetCities_ShouldThrowArgumentException_WhenPageSizeOrPageNumberIsLessThanOrEqualTo_Zero()
    {
        var cityName = "Nablus";
        var description = "Beautiful city";
        var invalidPageSize = -1;
        var invalidPageNumber = -1;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _cityService.GetCities(cityName, description, invalidPageSize, invalidPageNumber)
        );
    }

    [Fact]
    public async Task GetCities_Should_Throw_ArgumentException_When_CityName_And_Description_Are_Empty()
    {
        var cityName = string.Empty;
        var description = string.Empty;
        var pageSize = 10;
        var pageNumber = 1;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _cityService.GetCities(cityName, description, pageSize, pageNumber)
        );
    }

    [Fact]
    public async Task GetCities_Should_Increment_VisitCount_For_Each_City_That_Matches_Filter()
    {
        var cityName = "Nablus";
        var description = "A beautiful city in the northern West Bank.";
        var pageSize = 10;
        var pageNumber = 1;

        var cities = TestData.Cities;
        var filteredCities = cities.Where(c => c.Name.Contains(cityName)).ToList();
        var cityResponseDtos = TestData.CityResponseDtos;

        _mockUnitOfWork.Setup(u => u.CityRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<City, bool>>>(), pageSize, pageNumber))
            .ReturnsAsync(filteredCities);

        _mockUnitOfWork.Setup(u => u.CityRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<City>()))
            .Callback<int, City>((id, city) =>
            {
                var updatedCity = cities.First(c => c.CityID == id);
                updatedCity.VisitCount = city.VisitCount;
            });

        _mockMapper.Setup(m => m.Map<IEnumerable<CityResponseDto>>(It.IsAny<IEnumerable<City>>()))
            .Returns(cityResponseDtos);

        await _cityService.GetCities(cityName, description, pageSize, pageNumber);

        foreach (var city in filteredCities)
        {
            Assert.Equal(1, city.VisitCount);
        }

        foreach (var city in cities.Except(filteredCities))
        {
            Assert.Equal(0, city.VisitCount);
        }
    }

    [Fact]
    public async Task GetTopVisitedCitiesAsync_Should_Return_Top_Visited_Cities_In_Descending_VisitCount_Order()
    {
        var topCount = 2;
        var cities = new List<City>
            {
                new City { CityID = 1, Name = "Nablus", Country = "Palestine", Description = "A beautiful city in the northern West Bank.", VisitCount = 0 },
                new City { CityID = 2, Name = "Ramallah", Country = "Palestine", Description = "The administrative capital of Palestine.", VisitCount = 4 }
            };

        var cityResponseDtos = new List<CityResponseDto>
            {
                new CityResponseDto { CityID = 2, Name = "Ramallah", Country = "Palestine", Description = "The administrative capital of Palestine." },
                new CityResponseDto { CityID = 1, Name = "Nablus", Country = "Palestine", Description = "A beautiful city in the northern West Bank." }
            };

        _mockUnitOfWork.Setup(u => u.CityRepository.GetTopVisitedCitiesAsync(topCount))
            .ReturnsAsync(cities);

        _mockMapper.Setup(m => m.Map<IEnumerable<CityResponseDto>>(It.IsAny<IEnumerable<City>>()))
            .Returns(cityResponseDtos);

        var result = await _cityService.GetTopVisitedCitiesAsync(topCount);

        Assert.NotNull(result);
        var resultList = result.ToList();

        Assert.Equal("Ramallah", resultList[0].Name);
        Assert.Equal("Nablus", resultList[1].Name);
    }

    [Fact]
    public async Task GetTopVisitedCitiesAsync_Should_Throw_NotFoundException_When_No_Cities_Found()
    {
        var topCount = 2;

        _mockUnitOfWork.Setup(u => u.CityRepository.GetTopVisitedCitiesAsync(topCount))
            .ReturnsAsync((IEnumerable<City>)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _cityService.GetTopVisitedCitiesAsync(topCount));
        Assert.Equal("No cities found.", exception.Message);
    }

    [Fact]
    public async Task DeleteHotelFromCityAsync_Should_Remove_Hotel_From_City()
    {
        // Arrange
        var cityId = 1;
        var hotelId = 1;

        var hotel = new Hotel
        {
            HotelId = hotelId,
            CityID = cityId
        };

        var city = new City
        {
            CityID = cityId,
            Hotels = new List<Hotel> { hotel }
        };

        _mockUnitOfWork.Setup(u => u.CityRepository.GetByIdAsync(cityId))
            .ReturnsAsync(city);

        _mockUnitOfWork.Setup(u => u.HotelRepository.GetByIdAsync(hotelId))
            .ReturnsAsync(hotel);

        _mockUnitOfWork.Setup(u => u.HotelRepository.DeleteAsync(hotelId));

        // Act
        await _cityService.DeleteHotelFromCityAsync(cityId, hotelId);

        // Assert
        Assert.DoesNotContain(hotel, city.Hotels);
        _mockUnitOfWork.Verify(u => u.HotelRepository.DeleteAsync(hotelId), Times.Once);
    }

}