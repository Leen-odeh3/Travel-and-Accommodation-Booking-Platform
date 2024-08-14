using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository :GenericRepository<City> , ICityRepository
{
    private readonly ILogger _logger;
    public CityRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }

    public async Task<City> GetCityByIdAsync(int cityId, bool includeHotels = false)
    {
        ValidationHelper.ValidateId(cityId);

        IQueryable<City> query = _appDbContext.Cities.AsQueryable();

        if (includeHotels)
            query = query.Include(c => c.Hotels);

        return await query.FirstOrDefaultAsync(c => c.CityID == cityId);
    }
    public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount)
    {
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
