namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ImageRepository:GenericRepository<Image> , IImageRepository
{    public ImageRepository(AppDbContext context)
        : base(context)
    {
    }
    public async Task<Image> GetByPublicIdAsync(string publicId)
    {
        return await _appDbContext.Set<Image>().FirstOrDefaultAsync(img => img.PublicId == publicId);
    }
    public async Task<IEnumerable<Image>> GetImagesByTypeAsync(string type)
    {
        return await _appDbContext.Images.Where(img => img.Type == type).ToListAsync();
    }
}
