namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IImageService
{
    Task UploadImagesAsync(string entityType, int entityId, IList<IFormFile> files);
    Task<IEnumerable<object>> GetImagesAsync(string entityType, int entityId);
    Task DeleteImageAsync(string entityType, int entityId, int imageId);
    Task DeleteAllImagesAsync(string entityType, int entityId);
}
