namespace HotelBookingPlatform.Infrastructure.Implementation;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly AppDbContext _context;
    public HotelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    private IQueryable<Hotel> GetHotelsWithIncludes()
    {
        return _context.Hotels
            .Include(h => h.City)
            .Include(h => h.Owner)
            .Include(h => h.Reviews);
    }

    public async Task<IEnumerable<Hotel>> SearchCriteria(string name, string desc, int pageSize = 10, int pageNumber = 1)
    {
        return await GetHotelsWithIncludes()
            .Where(h => (string.IsNullOrEmpty(name) || h.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(desc) || h.Description.Contains(desc)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Hotel> GetHotelWithRoomClassesAndRoomsAsync(int hotelId)
    {
        return await _context.Hotels
            .Include(h => h.RoomClasses)
                .ThenInclude(rc => rc.Rooms)
            .FirstOrDefaultAsync(h => h.HotelId == hotelId);
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync(int pageSize, int pageNumber)
    {
        return await GetHotelsWithIncludes()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Hotel> GetByIdAsync(int id)
    {
        return await GetHotelsWithIncludes()
            .FirstOrDefaultAsync(h => h.HotelId == id);
    }

    public async Task<Hotel> GetHotelByNameAsync(string name)
    {
        return await _context.Hotels
            .Include(h => h.RoomClasses)
                .ThenInclude(rc => rc.Amenities)
            .FirstOrDefaultAsync(h => h.Name == name);
    }

    public async Task<IEnumerable<Hotel>> GetHotelsForCityAsync(int cityId)
    {
        return await _context.Hotels
            .Where(h => h.CityID == cityId)
            .ToListAsync();
    }

    public async Task<Hotel> GetHotelWithAmenitiesAsync(int hotelId)
    {
        return await _context.Hotels
            .Include(h => h.Amenities)
            .FirstOrDefaultAsync(h => h.HotelId == hotelId);
    }
}
