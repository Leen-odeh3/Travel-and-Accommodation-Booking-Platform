using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class BookingRepository :GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context):base(context)
    {
        
    }
    public async Task<IEnumerable<Booking>> GetBookingsWithDetailsAsync()
    {
        return await _appDbContext.Set<Booking>()
            .Include(b => b.User)
            .Include(b => b.Hotel)
            .ToListAsync();
    }

    public async Task<Booking> GetBookingWithDetailsAsync(int id)
    {
        return await _appDbContext.Set<Booking>()
            .Include(b => b.User)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.BookingID == id);
    }
}
