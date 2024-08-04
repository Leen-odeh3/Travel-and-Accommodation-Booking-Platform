using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;

    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Image>> GetImagesAsync(string entityType, int entityId)
    {
        return await _context.Images
            .Where(img => img.EntityType == entityType && (entityId == 0 || img.EntityId == entityId))
            .ToListAsync();
    }

    public async Task SaveImageAsync(string entityType, int entityId, byte[] imageData)
    {
        var existingImage = await _context.Images
            .FirstOrDefaultAsync(img => img.EntityType == entityType && img.EntityId == entityId);

        if (existingImage != null)
        {
            existingImage.FileData = imageData;
            _context.Images.Update(existingImage);
        }
        else
        {
            var newImage = new Image
            {
                EntityType = entityType,
                EntityId = entityId,
                FileData = imageData
            };
            await _context.Images.AddAsync(newImage);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteImagesAsync(string entityType, int entityId = 0)
    {
        var images = _context.Images
            .Where(img => img.EntityType == entityType && (entityId == 0 || img.EntityId == entityId));

        _context.Images.RemoveRange(images);
        await _context.SaveChangesAsync();
    }
}