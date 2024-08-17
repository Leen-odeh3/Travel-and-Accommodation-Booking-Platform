namespace HotelBookingPlatform.Infrastructure.Implementation;
public class BookingRepository :GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context)
        : base(context) { }
    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await _appDbContext.Bookings.FindAsync(bookingId);

        if (booking is null)
            throw new KeyNotFoundException("Booking not found.");

        if (booking.Status == BookingStatus.Completed && newStatus != BookingStatus.Completed)
            throw new InvalidOperationException("Cannot change the status of a completed booking.");

        booking.Status = newStatus;

        _appDbContext.Bookings.Update(booking);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Booking> GetByIdAsync(int id)
    {
        return await _appDbContext.Bookings
            .Include(b => b.Hotel) 
            .Include(b => b.Rooms)
            .Include(b => b.User).AsSplitQuery()
            .FirstOrDefaultAsync(b => b.BookingID == id);
    }
    public async Task<Booking> GetBookingByUserAndHotelAsync(string userId, int hotelId)
    {
        return await _appDbContext.Bookings
            .Where(b => b.UserId == userId && b.HotelId == hotelId)
            .FirstOrDefaultAsync();
    }
}

