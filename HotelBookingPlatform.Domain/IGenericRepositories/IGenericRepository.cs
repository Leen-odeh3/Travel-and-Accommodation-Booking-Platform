namespace HotelBookingPlatform.Domain.IRepositories;
public interface IGenericRepository<T> where T : class
{
     Task<IEnumerable<T>> GetAllAsyncPagenation(Expression<Func<T,bool>> filter,int PageSize=10 , int PageNumber=1);
     Task<T> GetByIdAsync(int id);
     Task<T> UpdateAsync(int id,T entity);
     Task<string> DeleteAsync(int id);
     Task<T> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
}
