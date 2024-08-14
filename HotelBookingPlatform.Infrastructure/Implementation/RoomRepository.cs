namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository :GenericRepository<Room> ,IRoomRepository
{
    private readonly ILogger _logger;
    public RoomRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }


}
