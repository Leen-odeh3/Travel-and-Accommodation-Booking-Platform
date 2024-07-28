using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IBookingService
{
    Task<Response<BookingDto>> GetBookingAsync(int id);
    Task<Response<BookingDto>> CreateBookingAsync(BookingCreateRequest request);
    Task<Response<string>> UpdateBookingAsync(int id, Booking booking);
    Task<Response<string>> DeleteBookingAsync(int id);
}