namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository :GenericRepository<Owner> ,IOwnerRepository
{
    private readonly ILog _logger;
    public OwnerRepository(AppDbContext context, ILog logger)
        : base(context, logger)
    {
        _logger = logger;
    }
    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        _logger.Log("Retrieving all owners with their associated hotels.","info");

        return await _appDbContext.owners
            .Include(h => h.Hotels) 
            .ToListAsync();
    }
}
