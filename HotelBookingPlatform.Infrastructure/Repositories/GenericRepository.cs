using HotelBookingPlatform.Domain.IRepositories;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _appDbContext;

    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _appDbContext.Set<T>().FindAsync(id);
        if (entity is not null)
        {
            _appDbContext.Set<T>().Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _appDbContext.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(int id, T entity)
    {
        var existingEntity = await _appDbContext.Set<T>().FindAsync(id);
        if (existingEntity is not null)
        {
            _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
