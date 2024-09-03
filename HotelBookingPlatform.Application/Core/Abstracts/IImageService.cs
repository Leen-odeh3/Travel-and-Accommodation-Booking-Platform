namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IImageService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string entityType, int entityId);
    Task<Image> GetImageByUniqueIdAsync(string uniqueId);
    Task<IEnumerable<Image>> GetImagesByTypeAsync(string type);
    Task<DeletionResult> DeleteImageAsync(string uniqueId);
}