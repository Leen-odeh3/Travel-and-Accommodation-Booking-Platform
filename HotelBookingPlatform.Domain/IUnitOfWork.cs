using HotelBookingPlatform.Domain.Abstracts;

namespace HotelBookingPlatform.Domain;
public interface IUnitOfWork 
{
    IHotelRepository HotelRepository { get; }
    IBookingRepository BookingRepository { get; }
    IRoomClasseRepository RoomClasseRepository { get; }
    IRoomRepository RoomRepository { get; }
    ICityRepository CityRepository { get; }
    Task<int> SaveChangesAsync();
}