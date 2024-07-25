using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomClassRepository :GenericRepository<RoomClass> , IRoomClasseRepository
{  
    private readonly AppDbContext _context;
    public RoomClassRepository(AppDbContext context) : base(context)
    {
        _context = context;

    }
    public async Task<IEnumerable<RoomClass>> SearchCriteria(
          string name,
          string desc,
          int pageSize = 10,
          int pageNumber = 1)
    {
        return await _context.RoomClasses
            .Include(h => h.Discounts)
            .Where(h => (string.IsNullOrEmpty(name) || h.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(desc) || h.Description.Contains(desc)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoomClass>> GetAllAsync(int pageSize, int pageNumber)
    {
        return await _context.RoomClasses
            .Include(h => h.Discounts)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<RoomClass> GetByIdAsync(int id)
    {
        return await _context.RoomClasses
            .Include(rc => rc.Discounts) 
            .FirstOrDefaultAsync(rc => rc.RoomClassID == id);
    }

}
