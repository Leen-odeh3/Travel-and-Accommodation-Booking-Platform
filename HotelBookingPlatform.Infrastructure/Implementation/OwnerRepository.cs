namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository :GenericRepository<Owner> ,IOwnerRepository
{
    public OwnerRepository(AppDbContext context) : base(context)
    {
    }
    public override async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _appDbContext.Set<Owner>()
            .Include(h => h.Hotels) 
            .ToListAsync();
    }
}
