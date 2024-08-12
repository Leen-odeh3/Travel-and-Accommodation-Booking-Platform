namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _context;
    public ImageRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteImageAsync(int imageId)
    {
        var image = await _context.Images
            .FirstOrDefaultAsync(img => img.Id == imageId);

        if (image is not null)
        {
            _context.Images.Remove(image);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Image>> GetImagesAsync(string entityType, int entityId)
    {
        return await _context.Images
            .Where(img => img.EntityType == entityType && img.EntityId == entityId)
            .ToListAsync();
    }

    public async Task SaveImagesAsync(string entityType, int entityId, IEnumerable<string> imageUrls)
    {
        foreach (var imageUrl in imageUrls)
        {
            var newImage = new Image
            {
                EntityType = entityType,
                EntityId = entityId,
                ImageUrl = imageUrl 
            };

            await _context.Images.AddAsync(newImage);
        }
        await _context.SaveChangesAsync();
    }


    public async Task DeleteImagesAsync(string entityType, int entityId = 0)
    {
        var images = _context.Images
            .Where(img => img.EntityType == entityType && img.EntityId == entityId);

        _context.Images.RemoveRange(images);
        await _context.SaveChangesAsync();
    }
}