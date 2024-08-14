namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository :GenericRepository<Owner> ,IOwnerRepository
{
    private readonly ILogger _logger;
    public OwnerRepository(AppDbContext context, ILogger logger)
        : base(context, logger)
    {
        _logger = logger;
    }
    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _appDbContext.owners
            .Include(h => h.Hotels) 
            .ToListAsync();
    }
}
