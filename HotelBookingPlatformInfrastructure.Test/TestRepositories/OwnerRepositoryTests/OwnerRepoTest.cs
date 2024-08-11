namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.OwnerRepositoryTests;
public class OwnerRepoTest
{
    private readonly OwnerRepository _sut;
    private readonly InMemoryDbContext _context;

    public OwnerRepoTest()
    {
        _context = new InMemoryDbContext();
        _sut = new OwnerRepository(_context);
    }
    private Owner GetSampleOwner(int id, string firstName, string lastName, string email, string phoneNumber, List<Hotel> hotels = null)
    {
        return new Owner
        {
            OwnerID = id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            Hotels = hotels ?? new List<Hotel>()

        };
    }
    private List<Owner> GetSampleOwners()
    {
        return new List<Owner>
        {
            GetSampleOwner(1, "John", "Doe", "john.doe@example.com", "123-456-7890"),
            GetSampleOwner(2, "Jane", "Smith", "jane.smith@example.com", "987-654-3210")
        };
    }

    [Fact]
    public async Task CreateOwner_ShouldCreateNewOwner()
    {
        // Arrange
        var owner = GetSampleOwner(1, "John", "Doe", "john.doe@example.com", "123-456-7890");

        // Act
        await _sut.CreateAsync(owner);
        var createdOwner = await _sut.GetByIdAsync(owner.OwnerID);

        // Assert
        Assert.NotNull(createdOwner);
        Assert.Equal(owner.FirstName, createdOwner.FirstName);
        Assert.Equal(owner.Email, createdOwner.Email);
    }

    [Fact]
    public async Task UpdateOwner_ShouldCorrectlyUpdateOwnerEmail()
    {
        // Arrange
        var oldOwner = GetSampleOwner(1, "Leen", "Odeh", "leen@info.com", "123-456-7890");
        await _sut.CreateAsync(oldOwner);

        var updatedOwner = GetSampleOwner(1, "Leen", "Odeh", "leen33@info.com", "123-456-7890");

        // Act
        await _sut.UpdateAsync(oldOwner.OwnerID, updatedOwner);
        var resultOwner = await _sut.GetByIdAsync(oldOwner.OwnerID);

        // Assert
        Assert.NotNull(resultOwner);
        Assert.Equal(updatedOwner.Email, resultOwner.Email);
        Assert.Equal(updatedOwner.FirstName, resultOwner.FirstName);
        Assert.Equal(updatedOwner.LastName, resultOwner.LastName);
    }

    [Fact]
    public async Task GetAllOwners_WhenListIsNotEmpty_ShouldReturnAllOwners()
    {
        // Arrange
        var owners = GetSampleOwners();
        foreach (var owner in owners)
        {
            await _sut.CreateAsync(owner);
        }

        // Act
        var resultOwners = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(resultOwners);
        Assert.Equal(owners.Count,2);
    }

    [Fact]
    public async Task GetAllAsync_ForOwner_ShouldReturnOwnersWithHotels()
    {
        // Arrange
        var hotels1 = new List<Hotel> { new Hotel { HotelId = 1, Name = "Sydney Coastal Retreat", PhoneNumber = "555-1234" } };
        var hotels2 = new List<Hotel> { new Hotel { HotelId = 2, Name = "Riverside Retreat", PhoneNumber = "444-1234" } };

        var owner1 = GetSampleOwner(1, "John", "Doe", "john.doe@example.com", "123-456-7890", hotels1);
        var owner2 = GetSampleOwner(2, "Jane", "Smith", "jane.smith@example.com", "987-654-3210", hotels2);

        await _sut.CreateAsync(owner1);
        await _sut.CreateAsync(owner2);

        // Act
        var owners = await _sut.GetAllAsync();

        var ownerList = owners.ToList();

        Assert.Equal("John", ownerList[0].FirstName);
        Assert.Single(ownerList[0].Hotels); 
        Assert.Equal("Sydney Coastal Retreat", ownerList[0].Hotels.First().Name);
        Assert.Equal("Riverside Retreat", ownerList[1].Hotels.First().Name);
    }
}
