namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ImageRepository : GenericRepository<Image>, IImageRepository
{
    public ImageRepository(AppDbContext context)
        : base(context) { }

    public async Task<Image> GetByUniqueIdAsync(string uniqueId)
    {
        return await _appDbContext.Images
            .FirstOrDefaultAsync(img => img.PublicId.Contains(uniqueId));
    }

    public async Task<IEnumerable<Image>> GetImagesByTypeAsync(string type)
    {
        return await _appDbContext.Images
            .Where(img => img.Type == type)
            .ToListAsync();
    }
    public async Task DeleteByUniqueIdAsync(string uniqueId)
    {
        var image = await GetByUniqueIdAsync(uniqueId);
        if (image is not null)
        {
            _appDbContext.Images.Remove(image);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
