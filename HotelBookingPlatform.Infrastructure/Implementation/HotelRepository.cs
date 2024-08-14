namespace HotelBookingPlatform.Infrastructure.Implementation;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly ILogger _logger;
    public HotelRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }

    private IQueryable<Hotel> GetHotelsWithIncludes()
    {
        return _appDbContext.Hotels
            .Include(h => h.City)
            .Include(h => h.Owner)
            .Include(h => h.Reviews).AsSplitQuery();
    }

    private async Task<IEnumerable<Hotel>> PaginateHotelsAsync(IQueryable<Hotel> query, int pageSize, int pageNumber)
    {
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Hotel>> SearchCriteria(string name, string desc, int pageSize = 10, int pageNumber = 1)
    {
        var query = GetHotelsWithIncludes()
            .Where(h => (string.IsNullOrEmpty(name) || h.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(desc) || h.Description.Contains(desc)));

        return await PaginateHotelsAsync(query, pageSize, pageNumber);
    }

    public async Task<Hotel> GetHotelWithRoomClassesAndRoomsAsync(int hotelId)
    {
        return await _appDbContext.Hotels
            .Include(h => h.RoomClasses)
                .ThenInclude(rc => rc.Rooms)
            .AsSplitQuery()
            .FirstOrDefaultAsync(h => h.HotelId == hotelId)
            ?? throw new KeyNotFoundException($"Hotel with ID {hotelId} not found.");
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync(int pageSize, int pageNumber)
    {
        return await PaginateHotelsAsync(GetHotelsWithIncludes(), pageSize, pageNumber);
    }

    public async Task<Hotel> GetByIdAsync(int id)
    {
        return await GetHotelsWithIncludes()
             .FirstOrDefaultAsync(h => h.HotelId == id)
             ?? throw new KeyNotFoundException($"Hotel with ID {id} not found.");
    }

    public async Task<Hotel> GetHotelByNameAsync(string name)
    {
        return await _appDbContext.Hotels
             .Include(h => h.RoomClasses)
                 .ThenInclude(rc => rc.Amenities)
             .AsSplitQuery()
             .FirstOrDefaultAsync(h => h.Name == name)
             ?? throw new KeyNotFoundException($"Hotel with name '{name}' not found.");
    }

    public async Task<IEnumerable<Hotel>> GetHotelsForCityAsync(int cityId)
    {
        return await _appDbContext.Hotels
            .Where(h => h.CityID == cityId)
            .ToListAsync();
    }

    public async Task<Hotel> GetHotelWithAmenitiesAsync(int hotelId)
    {
        return await _appDbContext.Hotels
              .Include(h => h.Amenities)
              .AsSplitQuery()
              .FirstOrDefaultAsync(h => h.HotelId == hotelId)
              ?? throw new KeyNotFoundException($"Hotel with ID {hotelId} not found.");
    }
}
