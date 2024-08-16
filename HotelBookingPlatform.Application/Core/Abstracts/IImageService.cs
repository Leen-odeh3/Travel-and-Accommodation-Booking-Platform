using CloudinaryDotNet.Actions;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IImageService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath, string imageType);
    Task<DeletionResult> DeleteImageAsync(string publicId);
    Task<GetResourceResult> GetImageDetailsAsync(string publicId);
}