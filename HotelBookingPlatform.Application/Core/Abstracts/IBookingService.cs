using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IBookingService
{
    Task<BookingDto> GetBookingAsync(int id);
    Task<BookingDto> CreateBookingAsync(BookingCreateRequest request);
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);

    // Task<string> UpdateBookingAsync(int id, Booking booking);
    // Task<string> DeleteBookingAsync(int id);
}