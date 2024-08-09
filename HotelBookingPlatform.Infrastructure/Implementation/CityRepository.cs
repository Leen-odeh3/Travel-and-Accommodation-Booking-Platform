using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.HelperMethods;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository :GenericRepository<City> , ICityRepository
{
    public CityRepository(AppDbContext context) : base(context)
    {
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
}
