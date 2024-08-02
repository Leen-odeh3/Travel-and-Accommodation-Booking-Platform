using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation
{
    public class RoomClassRepository : GenericRepository<RoomClass>, IRoomClasseRepository
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
                .Include(rc => rc.Discounts) 
                .Where(rc => (string.IsNullOrEmpty(name) || rc.Name.Contains(name)) &&
                             (string.IsNullOrEmpty(desc) || rc.Description.Contains(desc)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomClass>> GetAllAsync()
        {
            return await _context.RoomClasses
                .Include(rc => rc.Discounts) 
                .ToListAsync();
        }
        public async Task<RoomClass> GetByIdAsync(int id)
        {
            return await _context.RoomClasses
                .Include(rc => rc.Discounts) 
                .FirstOrDefaultAsync(rc => rc.RoomClassID == id);
        }

        public async Task<RoomClass> GetRoomClassWithAmenitiesAsync(int roomClassId)
        {
            return await _context.RoomClasses
                .Include(rc => rc.Amenities) // Ensure amenities are included
                .FirstOrDefaultAsync(rc => rc.RoomClassID == roomClassId);
        }

    }
}
