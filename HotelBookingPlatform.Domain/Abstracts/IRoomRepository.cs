namespace HotelBookingPlatform.Domain.Abstracts;
public interface IRoomRepository :IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAllIncludingAsync(params Expression<Func<Room, object>>[] includeProperties);
}
