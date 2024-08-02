using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository :GenericRepository<City> , ICityRepository
{
    private readonly AppDbContext _context;
    public CityRepository(AppDbContext context) : base(context)
    {
        _context= context;
    }
    public async Task<City> GetCityByIdAsync(int cityId, bool includeHotels = false)
    {
        IQueryable<City> query = _context.Cities.AsQueryable();

        if (includeHotels)
        {
            query = query.Include(c => c.Hotels);
        }

        return await query.FirstOrDefaultAsync(c => c.CityID == cityId);
    }
    public async Task DeleteAsync(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city != null)
        {
            _context.Cities.Remove(city);
        }
    }


    public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int topCount)
    {
        if (_context == null)
        {
            throw new InvalidOperationException("DbContext is not initialized.");
        }

        if (_context.Cities == null)
        {
            throw new InvalidOperationException("Cities DbSet is not initialized.");
        }
        return await _context.Cities
            .OrderByDescending(c => c.VisitCount) // Ensure VisitCount is a valid property
            .Take(topCount)
            .ToListAsync();
    }
}
