namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    private readonly ILogger _logger;
    public ReviewRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId)
    {
        return await _appDbContext.Reviews
            .Include(r => r.User) 
            .Include(r => r.Hotel).AsSplitQuery()
            .Where(r => r.HotelId == hotelId)
            .ToListAsync();
    }
}
