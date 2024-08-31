namespace HotelBookingPlatform.Domain.IRepositories;
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsyncPagenation(Expression<Func<T, bool>> filter = null, int PageSize = 10, int PageNumber = 1);
    Task<T> GetByIdAsync(int id);
    Task<T> UpdateAsync(int id, T entity);
    Task<string> DeleteAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
}
