using CloudinaryDotNet.Actions;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IImageService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folder);
    Task<DeletionResult> DeleteImageAsync(string publicId);
}