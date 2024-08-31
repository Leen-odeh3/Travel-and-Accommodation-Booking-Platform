namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IImageService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string imageType, int entityId);
    Task<DeletionResult> DeleteImageAsync(string publicId);
    Task<GetResourceResult> GetImageDetailsAsync(string publicId);
    Task<IEnumerable<Image>> GetImagesByTypeAsync(string type);
}