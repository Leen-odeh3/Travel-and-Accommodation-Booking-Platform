using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
{
     private readonly AppDbContext _context;

     public PhotoRepository(AppDbContext context) : base(context)
     {
            _context = context;
     }
     public async Task<IEnumerable<Photo>> GetPhotosByEntityIdAsync(int entityId, string entityType)
     {
            return await _context.Photos
                .Where(p => p.EntityId == entityId && p.EntityType == entityType)
                .ToListAsync();
     }


    public async Task DeletePhotoByIdAsync(int photoId)
    {
        var photo = await _context.Photos
            .FindAsync(photoId);

        if (photo is not null)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }
}
