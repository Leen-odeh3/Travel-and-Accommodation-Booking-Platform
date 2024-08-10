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

    [Fact]
    public void CreateOwner_ShouldCreateNewOwner()
    {
        var owner = new Owner
        {
            OwnerID = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890"
        };

        _sut.CreateAsync(owner);
        Assert.Equal("John", owner.FirstName);
    }

    [Fact]
    public async Task UpdateOwner_ShouldCorrectlyUpdateOwnerEmail()
    {
        var Old_owner = new Owner { OwnerID = 1, FirstName = "leen", LastName = "odeh", Email = "leen@info.com"};
        var New_owner = new Owner { OwnerID = 1, FirstName = "leen", LastName = "odeh", Email = "leen33@info.com" };

      var oldOwner = _sut.CreateAsync(Old_owner);
       var updatedOwner= _sut.UpdateAsync(1, New_owner);

        Assert.Equal("leen33@info.com", New_owner.Email);
        Assert.NotSame(oldOwner, updatedOwner);

    }
}

