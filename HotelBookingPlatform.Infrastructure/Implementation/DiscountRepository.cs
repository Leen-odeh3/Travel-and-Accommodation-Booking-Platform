namespace HotelBookingPlatform.Infrastructure.Implementation;
public class DiscountRepository : GenericRepository<Discount> ,IDiscountRepository
{
    public DiscountRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Discount>> GetAllAsync(Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null)
    {
        var query = _appDbContext.Set<Discount>().AsQueryable();
        query = query.Include(d => d.Room);

        return await query.ToListAsync();
    }

    public async Task<Discount> GetByIdAsync(int id, Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null)
    {
        var query = _appDbContext.Set<Discount>().AsQueryable();
        query = query.Include(d => d.Room);   

        return await query.FirstOrDefaultAsync(d => d.DiscountID == id);
    }

    public async Task DeleteAsync(int id)
    {
        var discount = await _appDbContext.Set<Discount>().FindAsync(id);
        if (discount != null)
        {
            _appDbContext.Set<Discount>().Remove(discount);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
