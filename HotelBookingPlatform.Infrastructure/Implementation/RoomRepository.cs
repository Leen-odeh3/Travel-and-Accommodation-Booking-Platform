namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository :GenericRepository<Room> ,IRoomRepository
{
    private readonly ILogger _logger;
    public RoomRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }
   public async Task<IEnumerable<Room>> GetAllIncludingAsync(params Expression<Func<Room, object>>[] includeProperties)
    {
        IQueryable<Room> query =_appDbContext.Rooms;

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }
}
