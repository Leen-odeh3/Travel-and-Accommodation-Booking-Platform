namespace HotelBookingPlatform.Domain.Abstracts;
public interface IBookingRepository:IGenericRepository<Booking>
{
    Task<Booking> GetByIdAsync(int id);
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);
    Task<Booking> GetBookingByUserAndHotelAsync(string userId, int hotelId);
}
