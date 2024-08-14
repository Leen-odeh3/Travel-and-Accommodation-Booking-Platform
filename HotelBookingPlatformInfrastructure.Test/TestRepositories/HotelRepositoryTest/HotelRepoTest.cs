namespace HotelBookingPlatform.Infrastructure.Test.TestRepositories.HotelRepositoryTest;
public class HotelRepoTest
{
    private readonly HotelRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly Mock<Domain.ILogger.ILogger> _logger;
    private readonly IFixture _fixture;

    public HotelRepoTest()
    {
        _context = new InMemoryDbContext();
        _logger = new Mock<Domain.ILogger.ILogger>();
        _sut = new HotelRepository(_context, _logger.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private City CreateCity(int id)
    {
        return _fixture.Build<City>()
            .With(c => c.CityID, id)
            .Create();
    }

    private Hotel CreateHotel(int cityId)
    {
        return _fixture.Build<Hotel>()
            .With(h => h.CityID, cityId)
            .Create();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddHotel()
    {
        // Arrange
        var city = CreateCity(1);
        var hotel = CreateHotel(1);
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.CreateAsync(hotel);

        // Assert
        var addedHotel = await _context.Hotels.FindAsync(result.HotelId);
        Assert.NotNull(addedHotel);
        Assert.Equal(hotel.Name, addedHotel.Name);
    }

    [Fact]
    public async Task GetHotelByNameAsync_ShouldReturnHotel_WhenHotelExists()
    {
        // Arrange
        var hotel = CreateHotel(1);
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelByNameAsync(hotel.Name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotel.Name, result.Name);
    }

    [Fact]
    public async Task GetHotelByNameAsync_ShouldThrowHotelNotFoundException_WhenHotelNotFound()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _sut.GetHotelByNameAsync("NonExistingHotelName")
        );
        Assert.Equal("Hotel with name 'NonExistingHotelName' not found.", exception.Message);
    }

    [Theory]
    [InlineData(9999)]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenHotelNotFound(int id)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _sut.GetByIdAsync(id)
        );
        Assert.Equal($"Hotel with ID {id} not found.", exception.Message);
    }

    [Theory]
    [InlineData(9999)]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetHotelsForCityAsync_ShouldReturnEmpty_WhenCityHasNoHotels(int cityId)
    {
        // Act
        var result = await _sut.GetHotelsForCityAsync(cityId);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetHotelsForCityAsync_ShouldReturnHotels_WhenCityHasHotels(int cityId)
    {
        // Arrange
        var city = CreateCity(cityId);
        var hotels = _fixture.Build<Hotel>()
            .With(h => h.CityID, cityId)
            .CreateMany(3)
            .ToList();

        _context.Cities.Add(city);
        _context.Hotels.AddRange(hotels);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelsForCityAsync(cityId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
        Assert.All(result, h => Assert.Equal(cityId, h.CityID));
    }

    [Fact]
    public async Task GetHotelWithAmenitiesAsync_ShouldReturnHotelWithAmenities_WhenHotelExists()
    {
        // Arrange
        var hotel = CreateHotel(1);
        hotel.Amenities = _fixture.CreateMany<Amenity>(7).ToList();
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelWithAmenitiesAsync(hotel.HotelId);

        // Assert
        Assert.Equal(hotel.HotelId, result.HotelId);
        Assert.Equal(hotel.Amenities.Count, result.Amenities.Count);
    }

    [Theory]
    [InlineData(9999)]
    public async Task GetHotelWithAmenitiesAsync_ShouldThrowKeyNotFoundException_WhenHotelNotFound(int hotelId)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _sut.GetHotelWithAmenitiesAsync(hotelId)
        );
        Assert.Equal($"Hotel with ID {hotelId} not found.", exception.Message);
    }
}
