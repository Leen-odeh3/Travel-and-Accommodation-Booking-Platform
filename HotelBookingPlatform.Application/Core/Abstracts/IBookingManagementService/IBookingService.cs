namespace HotelBookingPlatform.Application.Core.Abstracts.IBookingManagementService;
public interface IBookingService
{
    Task<BookingDto> GetBookingAsync(int id);
    Task<BookingDto> CreateBookingAsync(BookingCreateRequest request, string userId);
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);
}