namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository :GenericRepository<City> , ICityRepository
{
    private readonly ILog _logger;
    public CityRepository(AppDbContext context, ILog logger)
        : base(context, logger)
    {
        _logger = logger;
    }
    public async Task<City> GetCityByIdAsync(int cityId, bool includeHotels = false)
    {
        ValidationHelper.ValidateId(cityId);
        IQueryable<City> query = _appDbContext.Cities.AsQueryable();

        if (includeHotels)
            query = query.Include(c => c.Hotels).ThenInclude(h => h.Owner);

        return await query.FirstOrDefaultAsync(c => c.CityID == cityId);
    }
    public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount)
    {
        _logger.Log($"Retrieving top {topCount} visited cities.", "info");

        return await _appDbContext.Cities
            .OrderByDescending(c => c.VisitCount) 
            .Take(topCount)
            .ToListAsync();
    }
    public async Task CreateAsync(City city)
    {
        if (await _appDbContext.Cities.AnyAsync(c => c.Name == city.Name))
        {
            _logger.Log($"City with the name {city.Name} already exists.", "warning");
            throw new InvalidOperationException("City with the same name already exists.");
        }

        _appDbContext.Cities.Add(city);
        await _appDbContext.SaveChangesAsync();
        _logger.Log($"City {city.Name} created successfully.", "info");
    }
}
