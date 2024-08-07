using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Enums;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IBookingService
{
    Task<BookingDto> GetBookingAsync(int id);
    Task<BookingDto> CreateBookingAsync(BookingCreateRequest request);
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);

}