namespace HotelBookingPlatform.Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _appDbContext;
    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<string> DeleteAsync(int id)
    {
        var entity = await _appDbContext.Set<T>().FindAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");

        _appDbContext.Set<T>().Remove(entity);
        await _appDbContext.SaveChangesAsync();

        return "Entity deleted successfully.";
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsyncPagenation(Expression<Func<T, bool>> filter = null, int pageSize = 10, int pageNumber = 1)
    {
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        if (pageNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        IQueryable<T> query = _appDbContext.Set<T>();

        if (filter is not null)
            query = query.Where(filter);

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _appDbContext.Set<T>().FindAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");

        return entity;
    }

    public async Task<T> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _appDbContext.Set<T>().FindAsync(id);

        if (existingEntity is null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");

        _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _appDbContext.SaveChangesAsync();

        return existingEntity;
    }
}
