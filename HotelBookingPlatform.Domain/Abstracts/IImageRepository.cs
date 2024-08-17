namespace HotelBookingPlatform.Domain.Abstracts;
public interface IImageRepository :IGenericRepository<Image>
{
    Task<Image> GetByPublicIdAsync(string publicId);
    Task<IEnumerable<Image>> GetImagesByTypeAsync(string type);
}
