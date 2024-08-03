using Microsoft.AspNetCore.Http;

namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string[] allowedFileExtensions, string subFolder);
    Task<IList<string>> SaveFilesAsync(IList<IFormFile> files, string[] allowedFileExtensions, string subFolder);
    void DeleteFile(string fileName, string subFolder);
    Task<string> GetFilePathAsync(string fileName, string subFolder);
}