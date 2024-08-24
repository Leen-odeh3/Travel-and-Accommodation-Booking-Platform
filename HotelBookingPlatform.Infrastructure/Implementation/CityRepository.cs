namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(AppDbContext context)
        : base(context) { }
    public async Task<City> GetCityByIdAsync(int cityId, bool includeHotels = false)
    {
        IQueryable<City> query = _appDbContext.Cities.AsNoTracking();

        if (includeHotels)
            query = query.Include(c => c.Hotels).ThenInclude(h => h.Owner);

        return await query.SingleOrDefaultAsync(c => c.CityID == cityId);
    }
    public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount)
    {
        if (topCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(topCount), "The number of top cities must be greater than zero.");

        return await _appDbContext.Cities
            .OrderByDescending(c => c.VisitCount)
            .Take(topCount)
            .ToListAsync();
    }
    public async Task CreateAsync(City city)
    {
        if (await _appDbContext.Cities.AnyAsync(c => c.Name == city.Name))
            throw new InvalidOperationException("City with the same name already exists.");

        _appDbContext.Cities.Add(city);
        await _appDbContext.SaveChangesAsync();
    }
}
