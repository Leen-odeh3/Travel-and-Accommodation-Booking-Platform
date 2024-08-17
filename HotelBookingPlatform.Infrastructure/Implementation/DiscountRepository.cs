namespace HotelBookingPlatform.Infrastructure.Implementation;
public class DiscountRepository : GenericRepository<Discount> ,IDiscountRepository
{
    public DiscountRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Discount>> GetAllAsync(Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null)
    {
        var query = _appDbContext.Discounts.AsQueryable();
        query = query.Include(d => d.Room);

        return await query.ToListAsync();
    }
    public async Task<Discount> GetByIdAsync(int id, Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null)
    {
        var query = _appDbContext.Discounts.AsQueryable();
        query = query.Include(d => d.Room);   

        return await query.FirstOrDefaultAsync(d => d.DiscountID == id);
    }

    public async Task DeleteAsync(int id)
    {
        var discount = await _appDbContext.Discounts.FindAsync(id);
        if (discount is not null)
        {
            _appDbContext.Discounts.Remove(discount);
            await _appDbContext.SaveChangesAsync();
        }
    }
    public async Task<Discount> GetActiveDiscountForRoomAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        return await _appDbContext.Discounts
            .Where(d => d.RoomID == roomId && d.IsActive
                && d.StartDateUtc <= checkInDate
                && d.EndDateUtc >= checkOutDate)
            .FirstOrDefaultAsync();
    }

}
