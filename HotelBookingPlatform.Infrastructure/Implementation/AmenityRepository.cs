using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{
    private readonly AppDbContext _context;
    public AmenityRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Amenity>> GetAmenitiesByHotelNameAsync(string hotelName, int pageSize, int pageNumber)
    {
        return await _context.Amenities
            .Where(a => a.RoomClasses.Any(rc => rc.Hotel.Name.Contains(hotelName)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

