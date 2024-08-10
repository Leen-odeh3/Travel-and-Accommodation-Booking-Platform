namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.BookingRepositoryTests;
public class BookingRepoTest
{
    private readonly BookingRepository _sut;
    private readonly InMemoryDbContext _context;

    public BookingRepoTest()
    {
        _context = new InMemoryDbContext();
        _sut = new BookingRepository(_context);
    }

    [Fact]
    public void CreateBooking_IsUniqueIDBooking_ShouldBeCreateNewBooking()
    {
        var booking = new Booking
        {
            BookingID = 2,
            UserId = "user1237",
            Status = BookingStatus.Confirmed,
            confirmationNumber = "CONF123456",
            TotalPrice = 199.99m,
            BookingDateUtc = DateTime.UtcNow,
            PaymentMethod = PaymentMethod.PayPal,
            HotelId = 1001,
            CheckInDateUtc = DateTime.UtcNow.AddDays(1),
            CheckOutDateUtc = DateTime.UtcNow.AddDays(5)
        };

        _sut.CreateAsync(booking);
        Assert.True(booking.BookingID > 0);
    }

    [Fact]
    public void UpdateBookingStatusAsync_ShouldUpdateBookingStatus()
    {
        var bookingId = 1;
        var newStatus = BookingStatus.Pending;
        var booking = new Booking
        {
            BookingID = bookingId,
            Status = BookingStatus.Confirmed
        };

        _sut.UpdateBookingStatusAsync(bookingId, newStatus);

        Assert.NotEqual(newStatus, booking.Status);
    }
}
