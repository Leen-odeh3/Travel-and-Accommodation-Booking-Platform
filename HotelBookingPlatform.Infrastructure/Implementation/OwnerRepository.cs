namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository :GenericRepository<Owner> ,IOwnerRepository
{
    public OwnerRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<List<Owner>> GetAllAsync()
    {
        return await _appDbContext.Set<Owner>()
            .Include(o => o.Hotels) 
            .ToListAsync();
    }
}
