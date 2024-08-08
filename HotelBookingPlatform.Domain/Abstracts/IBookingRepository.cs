using HotelBookingPlatform.Domain.DTOs.Booking;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Enums;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IBookingRepository:IGenericRepository<Booking>
{
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);
    Task<Booking> GetBookingByUserAndHotelAsync(string userId, int hotelId);

}
