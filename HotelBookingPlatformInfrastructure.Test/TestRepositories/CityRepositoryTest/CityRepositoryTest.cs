namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.CityRepositoryTest;
public class CityRepoTest
{
    private readonly CityRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly Mock<ILog> _logger;
    private readonly IFixture _fixture;

    public CityRepoTest()
    {
        _context = new InMemoryDbContext();
        _logger = new Mock<ILog>();
        _sut = new CityRepository(_context, _logger.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private City CreateCity(int id) =>
        _fixture.Build<City>().With(c => c.CityID, id).Create();

    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnCity_WhenCityExists()
    {
        // Arrange
        var city = CreateCity(1);
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetCityByIdAsync(city.CityID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(city.CityID, result.CityID);
    }

    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnCityWithHotels_WhenIncludeHotelsIsTrue()
    {
        // Arrange
        var city = CreateCity(1);
        city.Hotels = _fixture.Build<Hotel>()
            .With(h => h.CityID, city.CityID)
            .CreateMany(3)
            .ToList();
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetCityByIdAsync(city.CityID, true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(city.CityID, result.CityID);
        Assert.NotEmpty(result.Hotels);
        Assert.All(result.Hotels, h => Assert.Equal(city.CityID, h.CityID));
    }

    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnNull_WhenCityDoesNotExist()
    {
        // Act
        var result = await _sut.GetCityByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTopVisitedCitiesAsync_ShouldReturnTopVisitedCities()
    {
        // Arrange
        var cities = _fixture.Build<City>()
            .With(c => c.VisitCount, 100)
            .CreateMany(10)
            .ToList();
        cities[0].VisitCount = 200;
        _context.Cities.AddRange(cities);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopVisitedCitiesAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(cities[0].CityID, result.First().CityID);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddCity_WhenCityDoesNotExist()
    {
        // Arrange
        var city = CreateCity(1);

        await _sut.CreateAsync(city);

        // Assert
        var addedCity = await _context.Cities.FindAsync(city.CityID);
        Assert.NotNull(addedCity);
        Assert.Equal(city.Name, addedCity.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenCityAlreadyExists()
    {
        // Arrange
        var city = CreateCity(1);
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HotelBookingPlatform.Domain.Exceptions.InvalidOperationException>(
            async () => await _sut.CreateAsync(city)
        );
        Assert.Equal("City with the same name already exists.", exception.Message);
    }
}