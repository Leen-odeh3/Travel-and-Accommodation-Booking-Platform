using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.IServices;
public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile file, string directoryName, string fileName);
    Task DeleteImageAsync(string directoryName, string fileName);
    Task<IEnumerable<string>> GetImageUrlsAsync(string directoryName);
}