using HotelBookingPlatform.Domain.Abstracts;

namespace HotelBookingPlatform.Domain;
public interface IUnitOfWork 
{
    IHotelRepository HotelRepository { get; }
    IBookingRepository BookingRepository { get; }
    Task<int> SaveChangesAsync();
}