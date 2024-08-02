using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IFileRepository
{
    Task<string> UploadFileAsync(IFormFile file, string folderPath);
    Task DeleteFileAsync(string filePath);
}
