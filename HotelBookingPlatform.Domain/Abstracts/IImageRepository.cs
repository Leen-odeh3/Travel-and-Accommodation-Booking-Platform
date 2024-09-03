namespace HotelBookingPlatform.Domain.Abstracts;
public interface IImageRepository : IGenericRepository<Image>
{
    Task<Image> GetByUniqueIdAsync(string uniqueId);
    Task<IEnumerable<Image>> GetImagesByTypeAsync(string type);
    Task DeleteByUniqueIdAsync(string uniqueId);
}