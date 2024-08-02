using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IRoomClasseRepository :IGenericRepository<RoomClass>
{
    Task<RoomClass> GetRoomClassWithAmenitiesAsync(int roomClassId);

}
