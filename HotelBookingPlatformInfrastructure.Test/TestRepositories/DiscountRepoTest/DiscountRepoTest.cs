namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.DiscountRepoTest;
public class DiscountRepoTest
{
    private readonly DiscountRepository _sut;
    private readonly InMemoryDbContext _context;

    public DiscountRepoTest()
    {
        _context = new InMemoryDbContext();
        _sut = new DiscountRepository(_context);
    }
    private void SeedDatabase()
    {
        _context.Rooms.Add(new Room
        {
            RoomID = 1,
            Number = "101" 
        });

        _context.Discounts.Add(new Discount
        {
            DiscountID = 1,
            RoomID = 1,
            Percentage = 10.0m,
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(10)
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDiscount()
    {
        var discountIdToDelete = 1;
        await _sut.DeleteAsync(discountIdToDelete);

        var deletedDiscount = await _context.Discounts.FindAsync(discountIdToDelete);
        Assert.Null(deletedDiscount);
    }
    [Fact]
    public async Task GetActiveDiscountForRoomAsync_ShouldReturnActiveDiscount()
    {
        // Arrange
        var discount = new Discount
        {
            RoomID = 1,
            Percentage = 15.0m,
            StartDateUtc = DateTime.UtcNow.AddDays(-1),
            EndDateUtc = DateTime.UtcNow.AddDays(1)
        };
        await _sut.CreateAsync(discount);
        await _context.SaveChangesAsync();

        // Act
        var activeDiscount = await _sut.GetActiveDiscountForRoomAsync(1, DateTime.UtcNow, DateTime.UtcNow);

        // Assert
        Assert.NotNull(activeDiscount);
        Assert.Equal(15.0m, activeDiscount.Percentage);
        Assert.Equal(1, activeDiscount.RoomID);
    }
}
