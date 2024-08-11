namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.CityRepositoryTests;
public class CityRepoTest
{
    private readonly CityRepository _sut;
    private readonly InMemoryDbContext _context;

    public CityRepoTest()
    {
        _context = new InMemoryDbContext();
        _sut = new CityRepository(_context);
    }

    [Fact]
    public async Task CreateCityAsync_ShouldAddCity()
    {
        var city = TestData.CreateCity("New York", "A major city in the United States.", "USA", "10001", 2);

        await _sut.CreateAsync(city);
        var result = await _sut.GetByIdAsync(city.CityID);

        Assert.NotNull(result);
        Assert.Equal("New York", result.Name);
    }

    [Fact]
    public async Task GetAllCitiesAsync_WhereNotNullCities_ShouldReturnAllCities()
    {
        var cities = await _sut.GetAllAsync();
        Assert.NotNull(cities);
        Assert.True(cities.Count() >= 0);
    }

    [Fact]
    public async Task GetCityByIdAsync_ForCityIsExist_ShouldReturnCity()
    {
        var city = TestData.CreateCity("Los Angeles", "A major city in California.", "USA", "90001", 0);

        await _sut.CreateAsync(city);

        var result = await _sut.GetByIdAsync(city.CityID);
        Assert.Equal(city.CityID, result.CityID);
    }

    [Theory]
    [InlineData("Chicago", "Chicago", "A major city in Illinois.", "60601", "A major city in Illinois.", "60602")]
    [InlineData("Los Angeles", "Los Angeles", "A major city in California.", "90001", "A major city in California.", "20300")]
    [InlineData("San Francisco", "San Jose", "A major city in California.", "94101", "California is the state of the city and the region is San Francisco Bay area.", "94102")]
    [InlineData("New York", "Miami", "A major city in the USA.", "10001", "vibrant in many ways that make it well worth a visit, from its colorful art deco architecture to its sun-drenched", "12007")]
    public async Task UpdateCityAsync_ShouldModifyCity(string initialName, string updatedName, string initialDescription, string initialPostOffice, string updatedDescription, string updatedPostOffice)
    {
        // Arrange
        var city = TestData.CreateCity(initialName, initialDescription, "USA", initialPostOffice, 0);

        await _sut.CreateAsync(city);

        // Act
        city.Name = updatedName;
        city.Description = updatedDescription;
        city.PostOffice = updatedPostOffice;
        await _sut.UpdateAsync(city.CityID, city);

        // Assert
        var result = await _sut.GetByIdAsync(city.CityID);
        Assert.NotNull(result);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(updatedDescription, result.Description);
        Assert.Equal(updatedPostOffice, result.PostOffice);
    }

    [Fact]
    public async Task GetTopVisitedCitiesAsync_ShouldReturnTopVisitedCities()
    {
        // Arrange
        var topCount = 3;
        var cities = TestData.GetTestCities();

        _context.Cities.AddRange(cities);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTopVisitedCitiesAsync(topCount);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(topCount, resultList.Count);
        Assert.Equal("Naples", resultList[0].Name);
        Assert.Equal("Florence", resultList[1].Name);
        Assert.Equal("Milan", resultList[2].Name);
    }
    [Fact]
    public async Task GetAllCitiesAsync_WhenNoCitiesExist_ShouldReturnEmptyList()
    {
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(result); 
        Assert.Empty(result);   
    }
}
