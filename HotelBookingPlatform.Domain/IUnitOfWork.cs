using HotelBookingPlatform.Domain.Abstracts;

namespace HotelBookingPlatform.Domain;
public interface IUnitOfWork 
{
    IHotelRepository HotelRepository { get; }
    IBookingRepository BookingRepository { get; }
    IRoomClasseRepository RoomClasseRepository { get; }
    IRoomRepository RoomRepository { get; }
    ICityRepository CityRepository { get; }
    IOwnerRepository OwnerRepository { get; }
    IDiscountRepository DiscountRepository { get; }
    Task<int> SaveChangesAsync();
}