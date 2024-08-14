namespace HotelBookingPlatform.Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _appDbContext;
    private readonly ILogger _logger;
    public GenericRepository(AppDbContext appDbContext, ILogger logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }
    public async Task<T> CreateAsync(T entity)
    {
        ValidationHelper.ValidateRequest(entity);
        await _appDbContext.Set<T>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        _logger.Log($"Entity of type {typeof(T).Name} created successfully.", "info");
        return entity;
    }
    public async Task<string> DeleteAsync(int id)
    {
        ValidationHelper.ValidateId(id);
        var entity = await _appDbContext.Set<T>().FindAsync(id);

        if (entity is null)
        {
            _logger.Log($"Entity with ID {id} not found for deletion.", "error");
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        _appDbContext.Set<T>().Remove(entity);
        await _appDbContext.SaveChangesAsync();

        _logger.Log($"Entity of type {typeof(T).Name} with ID {id} deleted successfully.", "info");
        return "Entity deleted successfully.";
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.Log($"Fetching all entities of type {typeof(T).Name}.", "info");
        return await _appDbContext.Set<T>().ToListAsync();
    }
    public async Task<IEnumerable<T>> GetAllAsyncPagenation(Expression<Func<T, bool>> filter = null, int pageSize = 10, int pageNumber = 1)
    {
        _logger.Log($"Fetching paginated entities of type {typeof(T).Name} with pageSize {pageSize} and pageNumber {pageNumber}.", "info");

        if (pageSize <= 0)
        {
            _logger.Log($"Invalid page size: {pageSize}. Must be greater than zero.", "error");
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
        }
        if (pageNumber <= 0)
        {
            _logger.Log($"Invalid page number: {pageNumber}. Must be greater than zero.", "error");
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
        }

        IQueryable<T> query = _appDbContext.Set<T>();

        if (filter is not null)
        {
            query = query.Where(filter);
            _logger.Log($"Applying filter to query.", "info");
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return await query.ToListAsync();
    }
    public async Task<T> GetByIdAsync(int id)
    {
        ValidationHelper.ValidateId(id);
        var entity = await _appDbContext.Set<T>().FindAsync(id);

        if (entity is null)
        {
            _logger.Log($"Entity of type {typeof(T).Name} with ID {id} not found.", "error");
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        _logger.Log($"Entity of type {typeof(T).Name} with ID {id} fetched successfully.", "info");
        return entity;
    }
    public async Task<T> UpdateAsync(int id, T entity)
    {
        ValidationHelper.ValidateId(id);
        ValidationHelper.ValidateRequest(entity);
        var existingEntity = await _appDbContext.Set<T>().FindAsync(id);

        if (existingEntity is null)
        {
            _logger.Log($"Entity of type {typeof(T).Name} with ID {id} not found for update.", "error");
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _appDbContext.SaveChangesAsync();

        _logger.Log($"Entity of type {typeof(T).Name} with ID {id} updated successfully.", "info");
        return existingEntity;
    }
}
