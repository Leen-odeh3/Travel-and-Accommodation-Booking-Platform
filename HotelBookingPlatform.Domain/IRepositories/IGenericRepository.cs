namespace HotelBookingPlatform.Domain.IRepositories;
public interface IGenericRepository<T> where T : class
{
     Task<IEnumerable<T>> GetAllAsync();
     Task<T> GetByIdAsync(int id);
     Task UpdateAsync(int id,T entity);
     Task DeleteAsync(int id);
     Task<T> CreateAsync(T entity);

}
