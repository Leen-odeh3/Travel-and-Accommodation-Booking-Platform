namespace HotelBookingPlatformApplication.Test.ServicesTest;
public class CityServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<City>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CityService _cityService;

    public CityServiceTest()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<City>>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();

        _cityService = new CityService(_unitOfWorkMock.Object, _mapperMock.Object, Mock.Of<ILog>());
    }

    [Fact]
    public async Task AddCityAsync_ShouldReturnCityResponseDto()
    {
        // Arrange
        var cityCreateRequest = _fixture.Create<CityCreateRequest>();
        var city = _fixture.Create<City>();
        var cityResponseDto = _fixture.Create<CityResponseDto>();

        _mapperMock.Setup(m => m.Map<City>(cityCreateRequest)).Returns(city);
        _mapperMock.Setup(m => m.Map<CityResponseDto>(city)).Returns(cityResponseDto);
        _unitOfWorkMock.Setup(u => u.CityRepository.CreateAsync(It.IsAny<City>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.AddCityAsync(cityCreateRequest);

        // Assert
        result.Should().BeEquivalentTo(cityResponseDto);
        _mapperMock.Verify(m => m.Map<City>(cityCreateRequest), Times.Once);
        _mapperMock.Verify(m => m.Map<CityResponseDto>(city), Times.Once);
        _unitOfWorkMock.Verify(u => u.CityRepository.CreateAsync(It.IsAny<City>()), Times.Once);
    }

    [Fact]
    public async Task GetCities_ShouldReturnCityResponseDtos()
    {
        // Arrange
        var cityName = "Ramallah";
        var description = "The administrative capital of the Palestinian territories.";
        var pageSize = 10;
        var pageNumber = 1;
        var cities = _fixture.CreateMany<City>(5).ToList();
        var cityResponseDtos = _fixture.CreateMany<CityResponseDto>(5).ToList();

        _unitOfWorkMock.Setup(u => u.CityRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<City, bool>>>(), pageSize, pageNumber))
            .ReturnsAsync(cities);
        _mapperMock.Setup(m => m.Map<IEnumerable<CityResponseDto>>(cities)).Returns(cityResponseDtos);

        // Act
        var result = await _cityService.GetCities(cityName, description, pageSize, pageNumber);

        // Assert
        result.Should().BeEquivalentTo(cityResponseDtos);
        _unitOfWorkMock.Verify(u => u.CityRepository.GetAllAsyncPagenation(It.IsAny<Expression<Func<City, bool>>>(), pageSize, pageNumber), Times.Once);
    }

    [Fact]
    public async Task GetCity_ShouldReturnCityWithHotelsResponseDto()
    {
        // Arrange
        var cityId = 1;
        var city = _fixture.Create<City>();
        var cityWithHotelsResponseDto = _fixture.Create<CityWithHotelsResponseDto>();

        _unitOfWorkMock.Setup(u => u.CityRepository.GetCityByIdAsync(cityId, true)).ReturnsAsync(city);
        _mapperMock.Setup(m => m.Map<CityWithHotelsResponseDto>(city)).Returns(cityWithHotelsResponseDto);

        // Act
        var result = await _cityService.GetCity(cityId, true);

        // Assert
        result.Should().BeEquivalentTo(cityWithHotelsResponseDto);
        _unitOfWorkMock.Verify(u => u.CityRepository.GetCityByIdAsync(cityId, true), Times.Once);
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnUpdatedCityResponseDto()
    {
        // Arrange
        var cityId = 1;
        var cityCreateRequest = _fixture.Create<CityCreateRequest>();
        var existingCity = _fixture.Create<City>();
        var updatedCity = _fixture.Create<City>();
        var cityResponseDto = _fixture.Create<CityResponseDto>();

        _unitOfWorkMock.Setup(u => u.CityRepository.GetByIdAsync(cityId)).ReturnsAsync(existingCity);
        _mapperMock.Setup(m => m.Map(cityCreateRequest, existingCity)).Returns(updatedCity);
        _mapperMock.Setup(m => m.Map<CityResponseDto>(updatedCity)).Returns(cityResponseDto);

        // Act
        var result = await _cityService.UpdateCity(cityId, cityCreateRequest);

        // Assert
        result.Should().BeEquivalentTo(cityResponseDto);
        _unitOfWorkMock.Verify(u => u.CityRepository.GetByIdAsync(cityId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CityRepository.UpdateAsync(cityId, updatedCity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteMethod()
    {
        var cityId = 1;

        _unitOfWorkMock.Setup(u => u.CityRepository.GetByIdAsync(cityId)).ReturnsAsync(_fixture.Create<City>());

        // Act
        await _cityService.DeleteAsync(cityId);

        // Assert
        _unitOfWorkMock.Verify(u => u.CityRepository.DeleteAsync(cityId), Times.Once);
    }

}