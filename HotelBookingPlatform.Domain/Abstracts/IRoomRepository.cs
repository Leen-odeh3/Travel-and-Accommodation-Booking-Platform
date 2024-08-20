namespace HotelBookingPlatform.Domain.Abstracts;
public interface IRoomRepository :IGenericRepository<Room>
{
    public Task<Room> GetByIdAsync(int id);
    Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Room>> GetAvailableRoomsWithNoBookingsAsync(int roomClassId);
}
