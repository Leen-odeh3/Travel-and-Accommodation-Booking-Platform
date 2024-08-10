namespace HotelBookingPlatform.Domain.Abstracts;
public interface IRoomClasseRepository :IGenericRepository<RoomClass>
{
    Task<RoomClass> GetRoomClassWithAmenitiesAsync(int roomClassId);
    Task<RoomClass> GetRoomClassWithRoomsAsync(int id);
}
