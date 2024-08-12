namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository :GenericRepository<Room> ,IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context)
    {
        
    }

}
