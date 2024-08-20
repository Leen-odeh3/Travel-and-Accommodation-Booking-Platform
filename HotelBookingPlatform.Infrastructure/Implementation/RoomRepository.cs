namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context)
        : base(context) { }
    public async Task<IEnumerable<Room>> GetRoomsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _appDbContext.Rooms.Include(rc => rc.RoomClass)
            .Where(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice)
            .ToListAsync();
    }
    public async Task<IEnumerable<Room>> GetAvailableRoomsWithNoBookingsAsync(int roomClassId)
    {
        var availableRooms = await _appDbContext.Rooms
            .Include(rc => rc.RoomClass)
            .Where(r => r.RoomClassID == roomClassId &&
                        !_appDbContext.Bookings.Any(b => b.Rooms.Contains(r)))
            .ToListAsync();

        return availableRooms;
    }
    public async Task<Room> GetByIdAsync(int id)
    {
        return await _appDbContext.Rooms
            .Include(r => r.RoomClass)
            .FirstOrDefaultAsync(r => r.RoomID == id);
    }
}
