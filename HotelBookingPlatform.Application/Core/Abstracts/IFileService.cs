using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderPath);
    Task DeleteFileAsync(string filePath);
    Task<IEnumerable<string>> GetFilesAsync(string folderPath);
}
