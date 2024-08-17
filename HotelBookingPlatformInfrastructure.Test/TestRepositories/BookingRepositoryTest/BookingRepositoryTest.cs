using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.BookingRepositoryTest;
public class BookingRepositoryTest
{
    private readonly BookingRepository _sut;
    private readonly AppDbContext _context;
    private readonly IFixture _fixture;

    public BookingRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _sut = new BookingRepository(_context);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldUpdateStatus()
    {
        // Arrange
        var booking = _fixture.Build<Booking>()
            .With(b => b.Status, BookingStatus.Pending)
            .Create();
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        await _sut.UpdateBookingStatusAsync(booking.BookingID, BookingStatus.Completed);

        // Assert
        var updatedBooking = await _context.Bookings.FindAsync(booking.BookingID);
        Assert.Equal(BookingStatus.Completed, updatedBooking.Status);
    }

    [Fact]
    public async Task UpdateBookingStatusAsync_ShouldThrowKeyNotFoundException_WhenBookingIsNull()
    {
        // Arrange
        var nonExistentBookingId = 999; 
        var newStatus = BookingStatus.Completed;
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _sut.UpdateBookingStatusAsync(nonExistentBookingId, newStatus));

        Assert.Equal("Booking not found.", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBooking()
    {
        // Arrange
        var booking = _fixture.Create<Booking>();
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(booking.BookingID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(booking.BookingID, result.BookingID);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        // Act
        var result = await _sut.GetByIdAsync(-1);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetBookingByUserAndHotelAsync_ShouldReturnNull()
    {
        // Act
        var result = await _sut.GetBookingByUserAndHotelAsync("28d54c8e-cde8-4c68-928e-83a0630f7be2", 999);

        // Assert
        Assert.Null(result);
    }

}
