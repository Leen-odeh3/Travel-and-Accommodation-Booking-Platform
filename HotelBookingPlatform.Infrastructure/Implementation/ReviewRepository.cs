namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext appDbContext) : base(appDbContext) { }
    public async Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId)
    {
        return await _appDbContext.Reviews
            .Include(r => r.User) 
            .Include(r => r.Hotel).AsSplitQuery()
            .Where(r => r.HotelId == hotelId)
            .ToListAsync();
    }
}
