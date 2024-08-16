namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    private readonly ILog _logger;
    public RoomRepository(AppDbContext context, ILog logger)
        : base(context, logger)
    {
        _logger = logger;
    }
}
