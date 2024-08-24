namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
{
    public OwnerRepository(AppDbContext context)
        : base(context) { }
    public async Task<IEnumerable<Owner>> GetAllWithHotelsAsync()
    {
        return await _appDbContext.owners
            .Include(h => h.Hotels)
            .ToListAsync();
    }
}
