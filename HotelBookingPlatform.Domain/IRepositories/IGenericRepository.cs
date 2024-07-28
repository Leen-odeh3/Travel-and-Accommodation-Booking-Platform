using System.Linq.Expressions;

namespace HotelBookingPlatform.Domain.IRepositories;
public interface IGenericRepository<T> where T : class
{
     Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter,int PageSize=10 , int PageNumber=1);
     Task<T> GetByIdAsync(int id);
     Task UpdateAsync(int id,T entity);
     Task DeleteAsync(int id);
     Task<T> CreateAsync(T entity);
}
