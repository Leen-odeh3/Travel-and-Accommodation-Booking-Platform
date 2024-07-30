using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly AppDbContext _context;
    public HotelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Hotel>> SearchCriteria(
        string name,
        string desc,
        int pageSize = 10,
        int pageNumber = 1)
    {
        return await _context.Hotels
            .Include(h => h.City).Include(h => h.Owner).Include(h => h.Reviews)
            .Where(h => (string.IsNullOrEmpty(name) || h.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(desc) || h.Description.Contains(desc)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync(int pageSize, int pageNumber)
    {
        return await _context.Hotels
            .Include(h => h.City).Include(h => h.Owner)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Hotel> GetByIdAsync(int id)
    {
        return await _context.Hotels
            .Include(h => h.City).Include(h => h.Reviews).Include(h => h.Owner)
            .FirstOrDefaultAsync(h => h.HotelId == id);
    }
    public async Task<Hotel> GetHotelByNameAsync(string name)
    {
        return await _context.Hotels
            .Include(h => h.RoomClasses).ThenInclude(xx=>xx.Amenities)
            .FirstOrDefaultAsync(h => h.Name == name);
    }
}
