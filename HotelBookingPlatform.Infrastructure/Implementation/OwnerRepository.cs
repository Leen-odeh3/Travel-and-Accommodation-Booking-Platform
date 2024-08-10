using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
