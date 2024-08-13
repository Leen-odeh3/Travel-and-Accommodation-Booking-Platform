namespace HotelBookingPlatform.Domain.Abstracts;
public interface IDiscountRepository :IGenericRepository<Discount>
{
    Task<IEnumerable<Discount>> GetAllAsync(Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null);
    Task<Discount> GetByIdAsync(int id, Expression<Func<IQueryable<Discount>, IQueryable<Discount>>> include = null);
    Task DeleteAsync(int id);
    Task<Discount> GetActiveDiscountForRoomAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
}
