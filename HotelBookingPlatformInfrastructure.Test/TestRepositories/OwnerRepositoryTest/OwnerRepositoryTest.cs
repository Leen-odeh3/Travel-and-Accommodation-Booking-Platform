﻿namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.OwnerRepositoryTest;
public class OwnerRepositoryTest
{
    private readonly OwnerRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly IFixture _fixture;

    public OwnerRepositoryTest()
    {
        _context = new InMemoryDbContext();
        _sut = new OwnerRepository(_context);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private Owner CreateOwner(int id) =>
        _fixture.Build<Owner>().With(o => o.OwnerID, id).Create();

    private Hotel CreateHotel(int ownerId) =>
        _fixture.Build<Hotel>().With(h => h.OwnerID, ownerId).Create();

    [Fact]
    public async Task GetAllWithHotelsAsync_ShouldReturnAllOwnersWithHotels()
    {
        // Arrange
        var owner1 = CreateOwner(1);
        var owner2 = CreateOwner(2);
        var hotelsForOwner1 = _fixture.Build<Hotel>()
            .With(h => h.OwnerID, owner1.OwnerID)
            .CreateMany(2).ToList();
        var hotelsForOwner2 = _fixture.Build<Hotel>()
            .With(h => h.OwnerID, owner2.OwnerID)
            .CreateMany(3).ToList();

        owner1.Hotels = hotelsForOwner1;
        owner2.Hotels = hotelsForOwner2;

        _context.owners.AddRange(owner1, owner2);
        _context.Hotels.AddRange(hotelsForOwner1.Concat(hotelsForOwner2));
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllWithHotelsAsync();

        // Assert
        var owners = result.ToList();
        Assert.Equal(2, owners.Count);
        Assert.Contains(owners, o => o.OwnerID == owner1.OwnerID && o.Hotels.Count == hotelsForOwner1.Count);
        Assert.Contains(owners, o => o.OwnerID == owner2.OwnerID && o.Hotels.Count == hotelsForOwner2.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOwner_WhenOwnerExists()
    {
        // Arrange
        var owner = CreateOwner(1);
        _context.owners.Add(owner);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(owner.OwnerID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(owner.OwnerID, result.OwnerID);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddOwner_WhenValidEntityIsProvided()
    {
        // Arrange
        var owner = CreateOwner(1);

        // Act
        await _sut.CreateAsync(owner);
        var createdOwner = await _context.owners.FindAsync(owner.OwnerID);

        // Assert
        Assert.NotNull(createdOwner);
        Assert.Equal(owner.OwnerID, createdOwner.OwnerID);
    }
}
